using System;
using Godot;

public partial class AudioEntryItem : Interactable
{
	public event Action OnCollect;
	public override string InteractionText => "Take";
	[Export] protected virtual AudioData audioData { get; set; }
	public override void _EnterTree()
	{
		base._EnterTree();
		if (audioData.IsCollected)
		{
			QueueFree();
		}
		else
		{
			audioData.OnCollected += QueueFree;
		}
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		audioData.OnCollected -= QueueFree;
	}
	public override void Interact()
	{
		base.Interact();
		AudioPlayerEvents.OnAudioCollect?.Invoke(audioData);
		OnCollect?.Invoke();
		audioData.IsCollected = true;
	}
}
