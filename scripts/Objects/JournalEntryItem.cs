
using System;
using Godot;

public partial class JournalEntryItem : Interactable
{
	public event Action OnCollect;
	public override string InteractionText => "Take";
	[Export] protected virtual JournalData journalData { get; set; }
	public override void _EnterTree()
	{
		base._EnterTree();
		if (journalData.IsCollected)
		{
			QueueFree();
		}
		else
		{
			journalData.OnCollected += QueueFree;
		}
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		journalData.OnCollected -= QueueFree;
	}
	public override void Interact()
	{
		base.Interact();
		JournalEvents.OnEntryCollect?.Invoke(journalData);
		OnCollect?.Invoke();
		journalData.IsCollected = true;
	}
}
