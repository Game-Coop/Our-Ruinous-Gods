
using System;
using Godot;

public class CollectableItem : Interactable
{
	public event Action OnCollect;
	public override string InteractionText => "Take";
	[Export] protected virtual ItemData itemData { get; set;}

	public override void Interact()
	{
		base.Interact();
		InventoryEvents.OnItemCollect?.Invoke(itemData);
		OnCollect?.Invoke();
		QueueFree();
	}
}
