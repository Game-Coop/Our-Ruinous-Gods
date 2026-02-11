using System.Collections.Generic;
using Godot;

[Tool]
public partial class EntryCreator : EditorPlugin
{
    private EditorFileDialog _saveDialog;
    private PopupMenu _popupMenu;
    private Button _dropdownButton;
    private int _selectedType = -1;
    private readonly string[] _creatableResources =
    {
        "Create Journal Entry",
        "Create Item Entry",
        "Create Audio Entry",
        "Create Puzzle Data",
    };
    public override void _EnterTree()
    {
        base._EnterTree();
        _dropdownButton = new Button { Text = "Create Resource ▼" };
        _dropdownButton.Pressed += OnDropdownPressed;
        AddControlToContainer(CustomControlContainer.Toolbar, _dropdownButton);

        _popupMenu = new PopupMenu();
        _popupMenu.AddItem("Journal Entry", 0);
        _popupMenu.AddItem("Item Entry", 1);
        _popupMenu.AddItem("Audio Entry", 2);
        _popupMenu.AddItem("Puzzle Data", 3);
        _popupMenu.IndexPressed += OnTypeSelected;

        _dropdownButton.AddChild(_popupMenu);
        // Save dialog
        _saveDialog = new EditorFileDialog
        {
            FileMode = EditorFileDialog.FileModeEnum.SaveFile,
            Access = EditorFileDialog.AccessEnum.Resources,
            Title = "Save JournalEntryData",
            CurrentPath = "res://resources",
            CurrentDir = "res://resources"
        };

        _saveDialog.AddFilter("*.tres", "JournalEntryData (*.tres)");
        _saveDialog.FileSelected += OnFileSelected;

        EditorInterface.Singleton.GetBaseControl().AddChild(_saveDialog);
    }
    private void OnDropdownPressed()
    {
        var rect = (Rect2I)_dropdownButton.GetRect();
        rect.Position += Vector2I.Down * (int)_dropdownButton.GetRect().Size.Y * 2;
        rect.Position += Vector2I.Right * 10;
        _popupMenu.Popup(rect);
    }

    private void OnTypeSelected(long index)
    {
        _selectedType = (int)index;
        OnCreatePressed();
    }
    public override void _DisablePlugin()
    {
        base._DisablePlugin();
        if (_dropdownButton != null)
            RemoveControlFromContainer(CustomControlContainer.Toolbar, _dropdownButton);
    }

    public override void _ExitTree()
    {
        _saveDialog.QueueFree();
    }

    private void OnCreatePressed()
    {
        _saveDialog.Title = _creatableResources[_selectedType];
        _saveDialog.ClearFilters();
        _saveDialog.AddFilter("*.tres");
        _saveDialog.CurrentFile = "";
        _saveDialog.PopupCenteredRatio();
    }

    private void OnFileSelected(string path)
    {
        CreateResource(path);
    }

    private void CreateResource(string path)
    {
        GameResource resource = null;
        if (_selectedType == 0)
        {
            resource = new JournalData
            {
                Id = MyResourceIdUtility.GetNextAvailableId<JournalData>()
            };
        }
        else if (_selectedType == 1)
        {
            resource = new ItemData
            {
                Id = MyResourceIdUtility.GetNextAvailableId<ItemData>()
            };
        }
        else if (_selectedType == 2)
        {
            resource = new AudioData
            {
                Id = MyResourceIdUtility.GetNextAvailableId<AudioData>()
            };
        }
        else if (_selectedType == 3)
        {
            resource = new PuzzleData
            {
                Id = MyResourceIdUtility.GetNextAvailableId<PuzzleData>()
            };
        }

        var err = ResourceSaver.Save(resource, path);
        if (err != Error.Ok)
        {
            GD.PushError($"Failed to save resource: {err}");
            return;
        }

        GD.Print($"Created resource at {path} with ID {resource.Id}");
        var addedResource = ResourceLoader.Load(path, cacheMode: ResourceLoader.CacheMode.Reuse);
        UpdateResources(addedResource);
        EditorInterface.Singleton.EditResource(addedResource);
    }

    private void UpdateResources(Resource addedResource)
    {
        if (ResourceFinder.TryFindResource<Resources>(out Resource resources))
        {
            if (_selectedType == 0)
            {
                var raw = (Godot.Collections.Array)resources.Get("journalDatas");
                RemoveDuplicates(raw);
                raw.Add(addedResource);
                resources.Set("journalDatas", raw);
            }
            else if (_selectedType == 1)
            {
                var raw = (Godot.Collections.Array)resources.Get("itemDatas");
                RemoveDuplicates(raw);
                raw.Add(addedResource);
                resources.Set("itemDatas", raw);
            }
            else if (_selectedType == 2)
            {
                var raw = (Godot.Collections.Array)resources.Get("audioDatas");
                RemoveDuplicates(raw);
                raw.Add(addedResource);
                resources.Set("audioDatas", raw);
            }
            else if (_selectedType == 3)
            {
                var raw = (Godot.Collections.Array)resources.Get("puzzleDatas");
                RemoveDuplicates(raw);
                raw.Add(addedResource);
                resources.Set("puzzleDatas", raw);
            }
            ResourceSaver.Save(resources, resources.ResourcePath);
        }
    }
    private void RemoveDuplicates(Godot.Collections.Array array)
    {
        var seenIds = new HashSet<int>();
        for (int i = array.Count - 1; i >= 0; i--)
        {
            var entry = array[i].As<Resource>();

            // Remove local resources (deleted files become local)
            if (string.IsNullOrEmpty(entry.ResourcePath))
            {
                array.RemoveAt(i);
                continue;
            }

            var id = (int)entry.Get("Id");
            if (!seenIds.Add(id))
            {
                array.RemoveAt(i);
            }
        }
    }
}
