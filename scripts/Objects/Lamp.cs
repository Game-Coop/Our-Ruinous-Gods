
using System;
using Godot;

public class Lamp : Spatial, ISwitchable
{
	public event Action<bool> OnStateChange;
	public bool IsOn { get; private set; }
	private Light light;
	public override void _Ready()
	{
		base._Ready();
		light = GetNode<SpotLight>("SpotLight");
	}
	public void Toggle()
	{
		if(IsOn) TurnOff();
		else TurnOn();
	}
	public void TurnOff()
	{
		GD.Print("Light turned off!");
		light.Visible = false;
		IsOn = false;
		OnStateChange?.Invoke(false);
	}

	public void TurnOn()
	{
		GD.Print("Light turned on!");
		light.Visible = true;
		IsOn = true;
		OnStateChange?.Invoke(true);
	}
}
