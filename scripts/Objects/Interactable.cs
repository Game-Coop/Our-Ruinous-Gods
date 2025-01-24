

using System;
using Godot;

public class Interactable : Spatial, IInteractable
{
	public virtual event Action<BaseEventData> OnInteract;
	public virtual string InteractionText => "Interact";
	[Export] public Texture hintIcon;
	private Hint _hint;
	private Hint hint
	{
		get
		{
			if (_hint == null)
			{
				_hint = GetNode<Hint>("Hint");
			}
			return _hint;
		}
	}
	public override void _Ready()
	{
		base._Ready();
		hint.Setup(InteractionText, hintIcon);
	}
	public virtual void HideHint()
	{
		if(IsInstanceValid(hint) == false) return;
		hint?.Hide();
	}

	public virtual void Interact()
	{
		OnInteract?.Invoke(null);
	}

	public virtual void ShowHint()
	{
		hint.Show();
	}
}
