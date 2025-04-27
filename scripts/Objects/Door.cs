
using System;
using Godot;

public class Door : Spatial, ISwitchable
{
    public event Action<bool> OnStateChange;
	[Export] public bool IsOn { get; private set; }

    public virtual void Toggle()
	{
		if(IsOn) TurnOff();
		else TurnOn();
	}
	public virtual void TurnOff()
	{
		IsOn = false;
		OnStateChange?.Invoke(false);
	}

	public virtual void TurnOn()
	{
		IsOn = true;
		OnStateChange?.Invoke(true);
	}
}