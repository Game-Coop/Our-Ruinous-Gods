
using System;
using Godot;

public partial class HandheldUI : Control
{
	[Export] private Button button;

	public override void _Ready()
	{
		base._Ready();
		button.Pressed += Button_Pressed;
	}

	private void Button_Pressed()
	{
		GD.Print("button pressed");
	}
}
