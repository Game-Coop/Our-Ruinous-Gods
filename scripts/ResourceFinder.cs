using Godot;

public static class ResourceFinder
{
    public static bool TryFindResource<T>(out Resource resource) where T : Resource, new()
    {
        var fs = EditorInterface.Singleton.GetResourceFilesystem();
        var root = fs.GetFilesystem();
        if (FindRecursive<T>(root, out resource))
        {
            return true;
        }
        return false;
    }
    // public T[] FindResources<T>()
    // {
    //     var fs = EditorInterface.Singleton.GetResourceFilesystem();
    //     var root = fs.GetFilesystem();
    // }

    private static bool FindRecursive<T>(EditorFileSystemDirectory dir, out Resource resource) where T : Resource, new()
    {
        resource = null;

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
                resource = res;
                return true;
            }
            else return false;
        }

        for (int i = 0; i < dir.GetSubdirCount(); i++)
        {
            if (FindRecursive<T>(dir.GetSubdir(i), out resource))
            {
                return true;
            }
        }
        return false;
    }
    private static string GetScriptPathForType<T>() where T : Resource, new()
    {
        // Create a temporary instance to grab its script path
        var temp = new T();
        var script = (Script)temp.GetScript();
        return script?.ResourcePath ?? "";
    }
}