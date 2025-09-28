using System.IO;

public partial class FileSaver : ISaver
{
    public FileSaver(string path, string extension)
    {
        this.path = path;
        this.extension = extension;
    }
    private string path;
    private string extension;
    public void Save(string saveName, string content)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        using (var sw = new StreamWriter(Path.Combine(path, $"{saveName}.{extension}")))
        {
            sw.Write(content);
        }
    }

    public string Load(string saveName)
    {
        string content = "";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        using (FileStream fs = new FileStream(Path.Combine(path, $"{saveName}.{extension}"), FileMode.OpenOrCreate))
        {
            if (fs != null)
            {
                StreamReader reader = new StreamReader(fs);
                content = reader.ReadToEnd();
            }
        }
        return content;
    }
    public FileInfo[] GetAvailableSaves(SaveFormat format)
    {
        DirectoryInfo d = new DirectoryInfo(path);
        return d.GetFiles($"*.{extension}");
    }
    public bool HasSave(string saveName)
    {
        if (File.Exists(Path.Combine(path, $"{saveName}.{extension}")))
        {
            return true;
        }
        return false;
    }
    public void Delete(string saveName)
    {
        string filePath = Path.Combine(path, $"{saveName}.{extension}");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}