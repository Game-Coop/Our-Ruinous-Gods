
using System;
using Godot;

public partial class Door : Node3D, ISwitchable
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