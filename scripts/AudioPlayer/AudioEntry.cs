using Godot;
using System;

public class AudioEntry : Button
{
	public event Action<AudioEntry> OnFocus;
	[Export] private NodePath labelPath;
	[Export] public AudioData audioData;
	public void Setup(AudioData audioData)
	{
		this.audioData = audioData;
	}
	public override void _Ready()
	{
		base._Ready();
		GetNode<Label>(labelPath).Text = audioData.Name;
	}

	public void OnFocused()
	{
		OnFocus?.Invoke(this);
	}

}
