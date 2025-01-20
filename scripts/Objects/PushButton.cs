
using System;
using Godot;

public class PushButton : Area, IInteractable
{
	public event Action<BaseEventData> OnInteract;
	[Export] private NodePath switchableObjectPath;
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
	private ISwitchable switchable;
	public string InteractionText => "Push";

	public override void _Ready()
	{
		base._Ready();
		switchable = GetNode<ISwitchable>(switchableObjectPath);
		hint.Setup(InteractionText, hintIcon);
	}
	public void Interact()
	{
		switchable.Toggle();
		OnInteract?.Invoke(null);
	}
	public void ShowHint()
	{
		hint.Show();
	}
	public void HideHint()
	{
		hint.Hide();
	}

}
