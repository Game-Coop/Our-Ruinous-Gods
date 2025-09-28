using System;
using Godot;

public partial class InteractionRaycaster : Node3D
{
	[Export] private NodePath rayCastPath;
	private RayCast3D rayCast;
	private IInteractable focusedInteractable;
	public override void _Ready()
	{
		base._Ready();
		rayCast = GetNode<RayCast3D>(rayCastPath);
	}
	public override void _Input(InputEvent e)
	{
		if (e.IsActionPressed("interact"))
		{
			if (focusedInteractable != null && focusedInteractable.CanInteract())
			{
				focusedInteractable.Interact();
			}
		}
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
		if (rayCast.IsColliding())
		{
			var collidingObject = rayCast.GetCollider();
			if (TryGetInteractable(collidingObject, out var interactable))
			{
				if (focusedInteractable != interactable)
				{
					focusedInteractable?.HideHint();
					interactable.ShowHint();
					focusedInteractable = interactable;
				}
			}
			else
			{
				focusedInteractable?.HideHint();
				focusedInteractable = null;
			}
		}
		else
		{
			focusedInteractable?.HideHint();
			focusedInteractable = null;
		}
	}
	public bool TryGetInteractable(GodotObject collidingObject, out IInteractable result)
	{
		if (collidingObject is IInteractable interactable && interactable.CanInteract())
		{
			result = interactable;
			return true;
		}
		if (collidingObject is Node collidingNode && collidingNode.GetParent() is IInteractable interactable1 && interactable1.CanInteract())
		{
			result = interactable1;
			return true;
		}
		result = null;
		return false;
	}
}
