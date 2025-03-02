using System;
using System.Collections.Generic;
using Godot;

public class Inventory : Page
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
	protected override void _Ready()
	{
		base._Ready();
		container = GetNode(itemsContainerPath);
		itemNameLabel = GetNode<Label>(itemNameLabelPath);
		itemDescriptionLabel = GetNode<Label>(itemDescriptionLabelPath);
		itemPreviewTextureRect = GetNode<TextureRect>(itemPreviewRectPath);

		InventoryEvents.OnItemCollect -= OnItemCollect;
		InventoryEvents.OnItemCollect += OnItemCollect;
	}
	public override void ShowPage(bool instant = false)
	{
		base.ShowPage(instant);
		GD.Print("Selected inventory");
		if (container.GetChildCount() > 0)
		{
			(container.GetChild(0) as InventoryItem).GrabFocus();
		}
	}
	private void OnItemCollect(ItemData data)
	{
		AddItem(data);
	}

	public void AddItem(ItemData itemData)
	{
		if (items.ContainsKey(itemData.Id))
		{
			GD.PrintErr("Item is already in inventory!");
			return;
		}
		var item = itemTemplate.Instance() as InventoryItem;
		item.Setup(itemData);
		item.OnFocus += OnItemFocus;
		items.Add(itemData.Id, item);

		container.AddChild(item);
		container.MoveChild(item, container.GetChildCount());
		// ConfigureFocus(item);
	}
	public void ConfigureFocus(InventoryItem item)
	{
		var childCount = container.GetChildCount();
		int index = item.GetIndex();
		var topItem = index - 1 >= 0 ? container.GetChildOrNull<InventoryItem>(index - 1) : null;
		var bottomItem = index + 1 < childCount ? container.GetChildOrNull<InventoryItem>(index + 1) : null;
		if (topItem != null)
		{
			item.FocusNeighbourTop = topItem.GetPath();
			topItem.FocusNeighbourBottom = item.GetPath();
		}
		if (bottomItem != null)
		{
			item.FocusNeighbourBottom = bottomItem.GetPath();
			bottomItem.FocusNeighbourTop = item.GetPath();
		}
	}
	public void RemoveItem(ItemData itemData)
	{
		if (items.ContainsKey(itemData.Id))
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
		if (item.itemData.Category == ItemCategory.Consumable)
		{
			// useBtn.Visible = true;
			// inspectBtn.Visible = true;
		}
	}
}
