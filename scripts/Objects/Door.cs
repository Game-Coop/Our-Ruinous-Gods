
using System;
using Godot;

public partial class Door : Node3D, ISwitchable, IPower
{
	public event Action<bool> OnStateChange;
	[Export] public bool IsOn { get; private set; }
	public PowerZone PowerZone { get; private set; }
	public virtual bool CanTurnOn => PowerZone == null || PowerZone.State == PowerState.On;
	public virtual void Toggle()
	{
		if (IsOn) TurnOff();
		else if (CanTurnOn) TurnOn();
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

	public void Register(PowerZone powerZone)
	{
		PowerZone = powerZone;
		powerZone.OnPowerChange += OnPowerChange;
		OnPowerChange(powerZone);
	}

	private void OnPowerChange(PowerZone zone)
	{
		if (zone.State == PowerState.Off)
			TurnOff();
	}
}