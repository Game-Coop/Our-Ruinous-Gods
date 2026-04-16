using Godot;

[GlobalClass]
public partial class AudioData : GameResource
{
	[Export] public AudioCategory Category { get; set; }
	[Export] public AudioStreamWav AudioStreamWAV { get; set; }
	public AudioData()
	{
		Id = 0;
		Name = "";
		Category = AudioCategory.AudioLogs;
		AudioStreamWAV = null;
	}
}
