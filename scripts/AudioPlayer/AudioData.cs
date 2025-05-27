using Godot;

[GlobalClass]
public partial class AudioData : Resource
{
    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; }
    [Export] public AudioCategory Category { get; set; }
    [Export] public AudioStreamWav AudioStreamWAV { get; set; }
    public bool IsCollected { get; set; }
    public AudioData()
    {
        Id = 0;
        Name = "";
        Category = AudioCategory.AudioLogs;
        AudioStreamWAV = null;
    }
}