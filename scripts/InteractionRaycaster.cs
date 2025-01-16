using System;
using Godot;

public class InteractionRaycaster : Spatial
{
	[Export] private NodePath rayCastPath;
	private RayCast rayCast;
	private IInteractable focusedInteractable;
	public override void _Ready()
	{
		base._Ready();
		rayCast = GetNode<RayCast>(rayCastPath);
	}
	public override void _Input(InputEvent e)
	{
		if (e.IsActionPressed("interact"))
		{
			focusedInteractable?.Interact();
		}
	}
	public override void _Process(float delta)
	{
		base._Process(delta);
		if (rayCast.IsColliding())
		{
			var collidingObject = rayCast.GetCollider();
			if (collidingObject is IInteractable interactable)
			{
				if (focusedInteractable != interactable)
				{
					focusedInteractable?.HideHint();
					interactable.ShowHint();
					focusedInteractable = interactable;
				}
			}
		}
		else
		{
			focusedInteractable?.HideHint();
			focusedInteractable = null;
		}
	}
}
