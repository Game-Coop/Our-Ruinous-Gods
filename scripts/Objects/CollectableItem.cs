
using System;
using Godot;

public partial class CollectableItem : Interactable
{
	public event Action OnCollect;
	public override string InteractionText => "Take";
	[Export] protected virtual ItemData itemData { get; set; }
	public override void _EnterTree()
	{
		base._EnterTree();
		if (itemData.IsCollected)
		{
			QueueFree();
		}
		else
		{
			itemData.OnCollected += QueueFree;
		}
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		itemData.OnCollected -= QueueFree;
	}
	public override void Interact()
	{
		base.Interact();
		InventoryEvents.OnItemCollect?.Invoke(itemData);
		OnCollect?.Invoke();
		itemData.IsCollected = true;
	}
}
