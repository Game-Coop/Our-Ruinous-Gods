using Godot;
using System.Collections.Generic;

public static class MyResourceIdUtility
{
    public static int GetNextAvailableId<T>() where T : GameResource, new()
    {
        var ids = new HashSet<int>();

        var fs = EditorInterface.Singleton.GetResourceFilesystem();
        var root = fs.GetFilesystem();

        CollectIdsRecursive<T>(root, ids);

        // Find smallest unused ID (0,1,2,3…)
        int nextId = 0;
        while (ids.Contains(nextId))
            nextId++;

        return nextId;
    }

    private static string GetScriptPathForType<T>() where T : GameResource, new()
    {
        // Create a temporary instance to grab its script path
        var temp = new T();
        var script = (Script)temp.GetScript();
        return script?.ResourcePath ?? "";
    }

    private static void CollectIdsRecursive<T>(EditorFileSystemDirectory dir, HashSet<int> ids) where T : GameResource, new()
    {
        var targetScriptPath = GetScriptPathForType<T>();

        for (int i = 0; i < dir.GetFileCount(); i++)
        {
            var path = dir.GetFilePath(i);

            if (!path.EndsWith(".tres") && !path.EndsWith(".res"))
                continue;

            var res = GD.Load<Resource>(path);
            var script = (Script)res.GetScript();

            if (script == null)
                continue;

            if (script.ResourcePath == targetScriptPath)
            {
                var id = (int)res.Get("Id");
                ids.Add(id);
            }
        }

        for (int i = 0; i < dir.GetSubdirCount(); i++)
            CollectIdsRecursive<T>(dir.GetSubdir(i), ids);
    }
}
