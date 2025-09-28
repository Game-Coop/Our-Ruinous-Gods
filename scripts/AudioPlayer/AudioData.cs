using System;
using Godot;

[GlobalClass]
public partial class AudioData : Resource
{
	[Export] public int Id { get; set; }
	[Export] public string Name { get; set; }
	[Export] public AudioCategory Category { get; set; }
	[Export] public AudioStreamWav AudioStreamWAV { get; set; }
	public event Action OnCollected;
	private bool _isCollected;
	public bool IsCollected
	{
		get { return _isCollected; }
		set
		{
			if (value != _isCollected)
			{
				_isCollected = value;
				if (_isCollected)
					OnCollected?.Invoke();
			}
		}
	}
	public AudioData()
	{
		Id = 0;
		Name = "";
		Category = AudioCategory.AudioLogs;
		AudioStreamWAV = null;
	}
}
