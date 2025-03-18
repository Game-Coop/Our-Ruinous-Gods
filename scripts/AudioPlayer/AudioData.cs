using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(AudioData), "", nameof(Resource))]
public class AudioData : Resource
{
    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; }
    [Export] public AudioCategory Category { get; set; }
    [Export] public AudioStreamSample AudioStreamSample { get; set; }

    public AudioData()
    {
        Id = 0;
        Name = "";
        Category = AudioCategory.AudioLogs;
        AudioStreamSample = null;
    }
}