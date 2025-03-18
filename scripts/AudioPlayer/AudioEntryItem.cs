using System;
using Godot;

public class AudioEntryItem : Interactable
{
	public event Action OnCollect;
	public override string InteractionText => "Take";
	[Export] protected virtual AudioData audioData { get; set;}

	public override void Interact()
	{
		base.Interact();
		AudioPlayerEvents.OnAudioCollect?.Invoke(audioData);
		OnCollect?.Invoke();
		QueueFree();
	}
}
