
using System;
using Godot;

public partial class InputBindingButton : Button
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

		Connect("pressed", new Callable(this, nameof(BindingPressed)));
	}
	public void ChangeIcon(Texture2D icon)
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
