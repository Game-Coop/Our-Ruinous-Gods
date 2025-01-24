
using System;
using Godot;

public class PushButton : Interactable
{
	[Export] private NodePath switchableObjectPath;
	private ISwitchable switchable;
	public override string InteractionText => "Push";
	public override void _Ready()
	{
		base._Ready();
		switchable = GetNode<ISwitchable>(switchableObjectPath);
	}
	public override void Interact()
	{
		base.Interact();
		switchable.Toggle();
	}
}
