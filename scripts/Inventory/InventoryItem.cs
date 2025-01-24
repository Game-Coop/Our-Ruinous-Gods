using Godot;
using System;

public class InventoryItem : Button
{
	public event Action<InventoryItem> OnFocus;
	[Export] private NodePath textureRectPath;
	[Export] private NodePath labelPath;
	[Export] public ItemData itemData;
	static int i = 0;
	public void Setup(ItemData itemData)
	{
		this.itemData = itemData;
	}
	public override void _Ready()
	{
		base._Ready();
		GetNode<TextureRect>(textureRectPath).Texture = itemData.IconSprite;
		GetNode<Label>(labelPath).Text = itemData.Name + (i++).ToString();
	}

	public void OnFocused()
	{
		OnFocus?.Invoke(this);
	}

}
