

using System;
using Godot;

public class Page : Control
{
	public bool IsSelected { get; private set; }
	public event Action OnSelect;
	public event Action OnUnSelect;
	protected virtual void _Ready()
	{
		base._Ready();
	}
	public override void _GuiInput(InputEvent @event)
	{
		if (!IsSelected)
		{
			GD.Print("Ignoring focus event");
			GetViewport().SetInputAsHandled();
		}
	}
	public virtual void Select()
	{
		// GrabFocus();
		var color = Modulate;
		color.a = 1f;
		Modulate = color;
		IsSelected = true;
		OnSelect?.Invoke();
	}
	public virtual void UnSelect()
	{
		// ReleaseFocus();
		var color = Modulate;
		color.a = 0f;
		Modulate = color;
		IsSelected = false;
		OnUnSelect?.Invoke();
	}
}
