
using System;
using Godot;

public partial class Lamp : Node3D, ISwitchable
{
	public event Action<bool> OnStateChange;
	public bool IsOn { get; private set; }
	private Light3D light;
	public override void _Ready()
	{
		base._Ready();
		light = GetNode<SpotLight3D>("SpotLight3D");
	}
	public void Toggle()
	{
		if(IsOn) TurnOff();
		else TurnOn();
	}
	public void TurnOff()
	{
		GD.Print("Light3D turned off!");
		light.Visible = false;
		IsOn = false;
		OnStateChange?.Invoke(false);
	}

	public void TurnOn()
	{
		GD.Print("Light3D turned on!");
		light.Visible = true;
		IsOn = true;
		OnStateChange?.Invoke(true);
	}
}
