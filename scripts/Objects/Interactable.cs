using System;
using Godot;

public class Interactable : Spatial, IInteractable
{
	public virtual event Action<BaseEventData> OnInteract;
	public virtual string InteractionText => "Interact";
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
		hint.Setup(InteractionText, InputBindings.GetKeyInfo("interact").Icon);
		hint.Show();
	}
}
