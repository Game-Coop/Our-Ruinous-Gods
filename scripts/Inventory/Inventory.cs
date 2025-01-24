using System;
using System.Collections.Generic;
using Godot;

public class Inventory : Control
{
	public Dictionary<int, InventoryItem> items = new Dictionary<int, InventoryItem>();
	[Export] private PackedScene itemTemplate;
	[Export] private NodePath itemsContainerPath;
	[Export] private NodePath itemPreviewRectPath;
	[Export] private NodePath itemNameLabelPath;
	[Export] private NodePath itemDescriptionLabelPath;

	private Node container;
	private Label itemNameLabel;
	private Label itemDescriptionLabel;
	private TextureRect itemPreviewTextureRect;
	private bool isShown = false;
	public override void _Ready()
	{
		base._Ready();

		container = GetNode(itemsContainerPath);
		itemNameLabel = GetNode<Label>(itemNameLabelPath);
		itemDescriptionLabel = GetNode<Label>(itemDescriptionLabelPath);
		itemPreviewTextureRect = GetNode<TextureRect>(itemPreviewRectPath);

		InventoryEvents.OnItemCollect -= OnItemCollect;
		InventoryEvents.OnItemCollect += OnItemCollect;
	}
	public override void _Input(InputEvent @event)
	{
		if(@event.IsActionPressed("inventory_toggle"))
		{
			if (isShown)
			{
				CloseInventory();
			}
			else
			{
				OpenInventory();
			}
			GetTree().SetInputAsHandled();
		}
	}

	private void OpenInventory()
	{
		isShown = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		Show();
		if (container.GetChildCount() > 0)
		{
			(container.GetChild(0) as InventoryItem).GrabFocus();
		}
	}
	private void CloseInventory()
	{
		isShown = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
		Hide();
	}

	private void OnItemCollect(ItemData data)
	{
		AddItem(data);
	}

	public void AddItem(ItemData itemData)
	{
		if(items.ContainsKey(itemData.Id))
		{
			GD.PrintErr("Item is already in inventory!");
			return;
		}
		var item = itemTemplate.Instance() as InventoryItem;
		item.Setup(itemData);
		item.OnFocus += OnItemFocus;
		items.Add(itemData.Id, item);
		
		container.AddChild(item);
		container.MoveChild(item, GetChildCount());
	}
	public void RemoveItem(ItemData itemData)
	{
		if(items.ContainsKey(itemData.Id))
		{
			items[itemData.Id].OnFocus -= OnItemFocus;
			items.Remove(itemData.Id);
		}
		else
		{
			GD.PrintErr("Item does not registered!");
		}
	}

	private void OnItemFocus(InventoryItem item)
	{
		itemPreviewTextureRect.Texture = item.itemData.PreviewSprite;
		itemDescriptionLabel.Text = item.itemData.Description;
		itemNameLabel.Text = item.itemData.Name;

		ConfigureButtons(item);
	}

	private void ConfigureButtons(InventoryItem item)
	{
		// useBtn.Visible = false;
		// inspectBtn.Visible = false;
		if(item.itemData.Category == ItemCategory.Consumable)
		{
			// useBtn.Visible = true;
			// inspectBtn.Visible = true;
		}
	}
}
