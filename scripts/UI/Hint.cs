using Godot;
using System;

public class Hint : Label3D
{
	[Export] private NodePath spritePath;

	private Sprite3D sprite;
	public override void _Ready()
	{
		base._Ready();
		sprite = GetNode<Sprite3D>(spritePath);
		Visible = false;
	}
	public void Setup(string text, Texture texture)
	{
		sprite.Texture = texture;
		Text = text;
	}
}
