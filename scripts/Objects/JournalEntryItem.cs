
using System;
using Godot;

public class JournalEntryItem : Interactable
{
	public event Action OnCollect;
	public override string InteractionText => "Take";
	[Export] protected virtual JournalData journalData { get; set;}

	public override void Interact()
	{
		base.Interact();
		JournalEvents.OnEntryCollect?.Invoke(journalData);
		OnCollect?.Invoke();
		QueueFree();
	}
}
