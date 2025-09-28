using System.IO;

public interface ISaver
{
    void Save(string saveName, string content);
    string Load(string saveName);
    void Delete(string saveName);
    FileInfo[] GetAvailableSaves(SaveFormat format);
    bool HasSave(string saveName);
}