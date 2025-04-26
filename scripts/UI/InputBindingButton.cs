
using System;
using Godot;

public class InputBindingButton : Button
{
	public event Action<InputBindingButton> OnBindingSelected;
	public event Action<InputBindingButton> OnBindingUnselected;
	[Export] public KeyInfo keyInfo { get; private set; }
	[Export] private NodePath selectionFramePath;
	private Control selectionFrame;

	public override void _Ready()
	{
		base._Ready();
		selectionFrame = GetNode<Control>(selectionFramePath);

		Connect("pressed", this, nameof(BindingPressed));
	}
	public void ChangeIcon(Texture icon)
	{
		Icon = icon;
	}
	private void BindingPressed()
	{
		Select();
	}
	public void Select()
	{
		selectionFrame.Visible = true;
		OnBindingSelected?.Invoke(this);
	}
	public void UnSelect()
	{
		selectionFrame.Visible = false;
		OnBindingUnselected?.Invoke(this);
	}
}
