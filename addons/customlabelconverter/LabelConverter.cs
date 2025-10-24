using Godot;
using System;
using System.Collections.Generic;

#if TOOLS
[Tool]
public partial class LabelConverter : EditorPlugin
{
    public override void _EnterTree()
    {
        AddToolMenuItem("Convert All Labels to CustomLabels (Project-Wide)", Callable.From(ConvertLabelsProjectWide));
        AddToolMenuItem("Convert All Labels to CustomLabels (Current Scene)", Callable.From(ConvertLabelsCurrentScene));
        GD.Print("LabelConverter enabled");
    }

    public override void _ExitTree()
    {
        RemoveToolMenuItem("Convert All Labels to CustomLabels (Project-Wide)");
        RemoveToolMenuItem("Convert All Labels to CustomLabels (Current Scene)");
        GD.Print("LabelConverter disabled");
    }

    private void ConvertLabelsCurrentScene()
    {
        var editorInterface = GetEditorInterface();
        var editedSceneRoot = editorInterface.GetEditedSceneRoot();

        if (editedSceneRoot == null)
        {
            GD.PrintErr("No scene is currently being edited");
            return;
        }

        GD.Print("Starting label conversion in current scene...");
        ProcessScene(editedSceneRoot, editedSceneRoot.SceneFilePath);
    }

    private void ConvertLabelsProjectWide()
    {
        GD.Print("========================================");
        GD.Print("Starting project-wide label conversion...");
        GD.Print("========================================");

        // Get all scene files in the project
        var sceneFiles = FindAllSceneFiles("res://");
        GD.Print($"Found {sceneFiles.Count} scene files in project");

        int totalConverted = 0;
        int scenesModified = 0;

        foreach (var scenePath in sceneFiles)
        {
            GD.Print($"\nProcessing: {scenePath}");

            // Load the scene
            var packedScene = GD.Load<PackedScene>(scenePath);
            if (packedScene == null)
            {
                GD.PrintErr($"  ✗ Failed to load scene: {scenePath}");
                continue;
            }

            // Instantiate the scene
            var sceneRoot = packedScene.Instantiate();
            if (sceneRoot == null)
            {
                GD.PrintErr($"  ✗ Failed to instantiate scene: {scenePath}");
                continue;
            }

            // Process the scene
            int converted = ProcessScene(sceneRoot, scenePath);

            if (converted > 0)
            {
                totalConverted += converted;
                scenesModified++;
            }

            // Clean up
            sceneRoot.QueueFree();
        }

        GD.Print("\n========================================");
        GD.Print($"✓ Conversion complete!");
        GD.Print($"  Scenes modified: {scenesModified}");
        GD.Print($"  Total labels converted: {totalConverted}");
        GD.Print("========================================");
    }

    private int ProcessScene(Node sceneRoot, string scenePath)
    {
        // Collect all Labels
        var labelsToReplace = new List<(Label label, Node parent, int index)>();
        CollectLabels(sceneRoot, labelsToReplace);

        if (labelsToReplace.Count == 0)
        {
            GD.Print("  No Label nodes found");
            return 0;
        }

        GD.Print($"  Found {labelsToReplace.Count} Label nodes");

        // Track old -> new node mappings for reference updates
        var nodeReplacements = new Dictionary<Node, Node>();

        // Process each label
        int convertedCount = 0;
        foreach (var (label, parent, index) in labelsToReplace)
        {
            try
            {
                var customLabel = new CustomLabel();
                label.ReplaceWithPreservingProperties(customLabel);

                // Set the owner so it gets saved with the scene
                customLabel.Owner = label.Owner ?? sceneRoot;

                // Track the replacement
                nodeReplacements[label] = customLabel;

                convertedCount++;
            }
            catch (Exception e)
            {
                GD.PrintErr($"  ✗ Failed to convert label '{label.Name}': {e.Message}");
            }
        }

        if (convertedCount > 0)
        {
            // Update all references to replaced nodes
            int referencesUpdated = UpdateNodeReferences(sceneRoot, nodeReplacements);
            if (referencesUpdated > 0)
            {
                GD.Print($"  ✓ Updated {referencesUpdated} node references");
            }

            // Save the modified scene
            var packedScene = new PackedScene();
            var result = packedScene.Pack(sceneRoot);

            if (result == Error.Ok)
            {
                result = ResourceSaver.Save(packedScene, scenePath);
                if (result == Error.Ok)
                {
                    GD.Print($"  ✓ Converted {convertedCount} labels and saved scene");
                }
                else
                {
                    GD.PrintErr($"  ✗ Failed to save scene: {result}");
                }
            }
            else
            {
                GD.PrintErr($"  ✗ Failed to pack scene: {result}");
            }
        }

        return convertedCount;
    }

    private int UpdateNodeReferences(Node root, Dictionary<Node, Node> replacements)
    {
        int updatedCount = 0;
        UpdateNodeReferencesRecursive(root, replacements, ref updatedCount);
        return updatedCount;
    }

    private void UpdateNodeReferencesRecursive(Node node, Dictionary<Node, Node> replacements, ref int updatedCount)
    {
        // Get all properties of this node
        foreach (var prop in node.GetPropertyList())
        {
            if (!prop.TryGetValue("usage", out var usageObj))
                continue;

            var usage = (PropertyUsageFlags)usageObj.AsInt32();

            // Only update exported/storage properties
            if (!usage.HasFlag(PropertyUsageFlags.Storage))
                continue;

            if (!prop.TryGetValue("name", out var nameObj))
                continue;

            var propName = new StringName(nameObj.ToString());

            try
            {
                var value = node.Get(propName);

                // Check if this property is a Node reference
                if (value.VariantType == Variant.Type.Object && value.AsGodotObject() is Node referencedNode)
                {
                    // If this node was replaced, update the reference
                    if (replacements.ContainsKey(referencedNode))
                    {
                        node.Set(propName, replacements[referencedNode]);
                        updatedCount++;
                        GD.Print($"    → Updated reference in {node.Name}.{propName}");
                    }
                }
                // Check if this property is an Array of Nodes
                else if (value.VariantType == Variant.Type.Array)
                {
                    var array = value.AsGodotArray();
                    bool arrayModified = false;

                    for (int i = 0; i < array.Count; i++)
                    {
                        if (array[i].VariantType == Variant.Type.Object && array[i].AsGodotObject() is Node arrayNode)
                        {
                            if (replacements.ContainsKey(arrayNode))
                            {
                                array[i] = replacements[arrayNode];
                                arrayModified = true;
                                updatedCount++;
                            }
                        }
                    }

                    if (arrayModified)
                    {
                        node.Set(propName, array);
                        GD.Print($"    → Updated array reference in {node.Name}.{propName}");
                    }
                }
            }
            catch
            {
                // Skip properties that can't be accessed
            }
        }

        // Recursively process children
        foreach (Node child in node.GetChildren())
        {
            UpdateNodeReferencesRecursive(child, replacements, ref updatedCount);
        }
    }

    private List<string> FindAllSceneFiles(string directory)
    {
        var sceneFiles = new List<string>();
        var dir = DirAccess.Open(directory);

        if (dir == null)
        {
            GD.PrintErr($"Failed to open directory: {directory}");
            return sceneFiles;
        }

        dir.ListDirBegin();
        string fileName = dir.GetNext();

        while (!string.IsNullOrEmpty(fileName))
        {
            if (fileName == "." || fileName == "..")
            {
                fileName = dir.GetNext();
                continue;
            }

            string fullPath = directory.PathJoin(fileName);

            if (dir.CurrentIsDir())
            {
                // Skip .godot and addons directories to avoid system files
                if (fileName != ".godot" && fileName != ".import")
                {
                    sceneFiles.AddRange(FindAllSceneFiles(fullPath));
                }
            }
            else if (fileName.EndsWith(".tscn") || fileName.EndsWith(".scn"))
            {
                sceneFiles.Add(fullPath);
            }

            fileName = dir.GetNext();
        }

        dir.ListDirEnd();
        return sceneFiles;
    }

    private void CollectLabels(Node node, List<(Label, Node, int)> labels)
    {
        // Only collect pure Label types, not derived classes like CustomLabel
        if (node.GetType() == typeof(Label))
        {
            var parent = node.GetParent();
            var index = node.GetIndex();
            labels.Add(((Label)node, parent, index));
        }

        // Recursively check children
        foreach (Node child in node.GetChildren())
        {
            CollectLabels(child, labels);
        }
    }
}

// Extension method for node replacement
public static class NodeReplacementExtensions
{
    public static void ReplaceWithPreservingProperties(this Node oldNode, Node newNode, bool keepProperties = true)
    {
        var parent = oldNode.GetParent();
        if (parent == null)
        {
            throw new InvalidOperationException($"Node '{oldNode.Name}' has no parent");
        }

        // Store metadata before replacement
        string nodeName = oldNode.Name;
        Node owner = oldNode.Owner;
        Vector2 savedSize = default;
        bool isControl = oldNode is Control;
        float anchorTop = 0f;
        float anchorLeft = 0f;
        float anchorRight = 0f;
        float anchorBottom = 0f;
        Vector2 pivotOffset = Vector2.Zero;
        if (isControl)
        {
            var control = (Control)oldNode;
            savedSize = control.Size;
            anchorTop = control.AnchorTop;
            anchorLeft = control.AnchorLeft;
            anchorRight = control.AnchorRight;
            anchorBottom = control.AnchorBottom;
            pivotOffset = control.PivotOffset;
        }

        if (keepProperties)
        {
            CopyProperties(oldNode, newNode);
        }

        int index = oldNode.GetIndex();
        parent.RemoveChild(oldNode);
        parent.AddChild(newNode);
        parent.MoveChild(newNode, index);
        oldNode.QueueFree();

        // Restore metadata
        newNode.Name = nodeName;
        newNode.Owner = owner;

        if (isControl && newNode is Control newControl)
        {
            newControl.Size = savedSize;
            newControl.AnchorTop = anchorTop;
            newControl.AnchorLeft = anchorLeft;
            newControl.AnchorRight = anchorRight;
            newControl.AnchorBottom = anchorBottom;
            newControl.PivotOffset = pivotOffset;
        }

        // Copy children's ownership
        CopyChildrenOwnership(oldNode, newNode);

        // Trigger transform updates for children
        RefreshChildTransforms(newNode);
    }

    private static void CopyProperties(Node oldNode, Node newNode)
    {
        Node defaultNode;
        try
        {
            defaultNode = (Node)ClassDB.Instantiate(oldNode.GetClass());
        }
        catch
        {
            return;
        }

        var newNodeProps = new HashSet<StringName>();
        foreach (var prop in newNode.GetPropertyList())
        {
            if (IsStorageProperty(prop))
            {
                if (prop.TryGetValue("name", out var nameObj))
                {
                    newNodeProps.Add(new StringName(nameObj.ToString()));
                }
            }
        }

        foreach (var prop in oldNode.GetPropertyList())
        {
            if (!IsStorageProperty(prop))
                continue;

            if (!prop.TryGetValue("name", out var nameObj))
                continue;

            var propName = new StringName(nameObj.ToString());

            if (!newNodeProps.Contains(propName))
                continue;

            try
            {
                var currentValue = oldNode.Get(propName);
                var defaultValue = defaultNode.Get(propName);

                if (!Variant.Equals(currentValue, defaultValue))
                {
                    newNode.Set(propName, currentValue);
                }
            }
            catch
            {
                // Skip properties that fail
            }
        }

        defaultNode.Free();
    }

    private static bool IsStorageProperty(Godot.Collections.Dictionary prop)
    {
        if (prop.TryGetValue("usage", out var usageObj))
        {
            var usage = (PropertyUsageFlags)usageObj.AsInt32();
            return usage.HasFlag(PropertyUsageFlags.Storage);
        }
        return false;
    }

    private static void ReconnectSignals(Node oldNode, Node newNode)
    {
        foreach (var signal in oldNode.GetSignalList())
        {
            if (!signal.TryGetValue("name", out var signalNameObj))
                continue;

            var signalName = new StringName(signalNameObj.ToString());

            if (!newNode.HasSignal(signalName))
                continue;

            var connections = oldNode.GetSignalConnectionList(signalName);

            foreach (var connection in connections)
            {
                if (!connection.TryGetValue("flags", out var flagsObj))
                    continue;

                var flags = (uint)flagsObj.AsInt32();

                if ((flags & (uint)GodotObject.ConnectFlags.Persist) != 0)
                {
                    if (connection.TryGetValue("callable", out var callableObj))
                    {
                        try
                        {
                            var callable = callableObj.AsCallable();
                            newNode.Connect(signalName, callable, flags);
                        }
                        catch
                        {
                            // Ignore connection errors
                        }
                    }
                }
            }
        }
    }

    private static void CopyChildrenOwnership(Node oldNode, Node newNode)
    {
        // When ReplaceBy is used, children are moved automatically
        // We just need to ensure their ownership is preserved
        for (int i = 0; i < newNode.GetChildCount(); i++)
        {
            var child = newNode.GetChild(i);
            if (child.Owner == oldNode)
            {
                child.Owner = newNode.Owner;
            }
        }
    }

    private static void RefreshChildTransforms(Node node)
    {
        for (int i = 0; i < node.GetChildCount(); i++)
        {
            var child = node.GetChild(i);
            if (child.HasMethod("get_transform") && child.HasMethod("set_transform"))
            {
                try
                {
                    var transform = child.Call("get_transform");
                    child.Call("set_transform", transform);
                }
                catch
                {
                    // Ignore errors
                }
            }
        }
    }
}
#endif