using Godot;
using System;

public class InventoryEntry : Button
{
	public event Action<InventoryEntry> OnFocus;
	[Export] private NodePath textureRectPath;
	[Export] private NodePath labelPath;
	[Export] public ItemData itemData;
	public void Setup(ItemData itemData)
	{
		this.itemData = itemData;
	}
	public override void _Ready()
	{
		base._Ready();
		GetNode<TextureRect>(textureRectPath).Texture = itemData.IconSprite;
		GetNode<Label>(labelPath).Text = itemData.Name;
	}

	public void OnFocused()
	{
		OnFocus?.Invoke(this);
	}

}
