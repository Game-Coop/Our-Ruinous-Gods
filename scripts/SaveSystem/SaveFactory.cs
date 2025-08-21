using System;
using Godot;

public static class SaveFactory
{
    public static ISaveSystem CreateLocalSaveSystem(Node nodeRef, string path, SaveFormat saveFormat)
    {
        return new SaveSystem(nodeRef, new FileSaver(path, GetExtension(saveFormat)), CreateDecoder(saveFormat));
    }

    private static string GetExtension(SaveFormat saveFormat)
    {
        if (saveFormat == SaveFormat.Json)
        {
            return "json";
        }
        else if (saveFormat == SaveFormat.Binary)
        {
            return "dat";
        }
        else return "txt";
    }

    private static IDecoder CreateDecoder(SaveFormat saveFormat)
    {
        if (saveFormat == SaveFormat.Json)
            return new JsonDecoder();
        else return null;
    }
}
