using Godot;
using System;

public class ButtonManager : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get the button node
		Button btn = GetNode<Button>("Button");
	}

	public void OnButtonPressed()
	{
		GD.Print("Pressed");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
