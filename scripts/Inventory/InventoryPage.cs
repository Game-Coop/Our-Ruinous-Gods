using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class InventoryPage : Page
{
	private Dictionary<int, InventoryEntry> inventoryEntries = new Dictionary<int, InventoryEntry>();
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
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		InventoryEvents.OnInventoryChange += UpdateInventory;
		
		InventoryEvents.OnUpdateRequest.Invoke();
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		InventoryEvents.OnInventoryChange -= UpdateInventory;
	}
	public override void ShowPage(bool instant = false)
	{
		base.ShowPage(instant);
		if (container.GetChildCount() > 0)
		{
			(container.GetChild(0) as InventoryEntry).GrabFocus();
		}
	}
	private void UpdateInventory(Dictionary<int, ItemData> itemDatas)
	{
		var entriesToRemove = inventoryEntries.Where(pair => !itemDatas.ContainsKey(pair.Key));
		foreach (var entry in entriesToRemove)
		{
			RemoveEntry(entry.Value.itemData);
		}

		var entriesToAdd = itemDatas.Where(pair => !inventoryEntries.ContainsKey(pair.Key));

		foreach (var item in entriesToAdd)
		{
			AddEntry(item.Value);
		}
	}

	public void AddEntry(ItemData itemData)
	{
		if (inventoryEntries.ContainsKey(itemData.Id))
		{
			GD.PrintErr("Item is already in inventory!");
			return;
		}
		var item = itemTemplate.Instance() as InventoryEntry;
		item.Setup(itemData);
		item.OnFocus += OnItemFocus;
		inventoryEntries.Add(itemData.Id, item);

		container.AddChild(item);
		container.MoveChild(item, container.GetChildCount());
		// ConfigureFocus(item);
	}
	public void ConfigureFocus(InventoryEntry item)
	{
		var childCount = container.GetChildCount();
		int index = item.GetIndex();
		var topItem = index - 1 >= 0 ? container.GetChildOrNull<InventoryEntry>(index - 1) : null;
		var bottomItem = index + 1 < childCount ? container.GetChildOrNull<InventoryEntry>(index + 1) : null;
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
	public void RemoveEntry(ItemData itemData)
	{
		if (inventoryEntries.ContainsKey(itemData.Id))
		{
			var entry = inventoryEntries[itemData.Id];
			entry.OnFocus -= OnItemFocus;
			inventoryEntries.Remove(itemData.Id);
			entry.QueueFree();
		}
		else
		{
			GD.PrintErr("Item does not registered!");
		}
	}
	private void OnItemFocus(InventoryEntry item)
	{
		itemPreviewTextureRect.Texture = item.itemData.PreviewSprite;
		itemDescriptionLabel.Text = item.itemData.Description;
		itemNameLabel.Text = item.itemData.Name;

		ConfigureButtons(item);
	}
	private void ConfigureButtons(InventoryEntry item)
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
