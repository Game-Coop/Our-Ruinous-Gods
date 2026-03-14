using Godot;
using System;

public partial class PowerableLight : Node3D, IPower
{
	[Export] private Node3D light;
	[Export] public PowerState State { get; set; }
	public PowerZone PowerZone { get; private set; }

	private EventBus eventBusHandler;
	public void Register(PowerZone powerZone)
	{
		eventBusHandler = GetNode<EventBus>("/root/EventBus");

		PowerZone = powerZone;
		powerZone.OnPowerChange += OnPowerChange;
		OnPowerChange(powerZone);
	}
	private void OnPowerChange(PowerZone powerZone)
	{
		if (State == PowerState.On && powerZone.State == PowerState.On)
		{
			light.Show();
		}
		else
		{
			light.Hide();
		}
		PowerEvent newEvent = new PowerEvent();
		newEvent.PowerZone = PowerZone;
		eventBusHandler.OnPowerChangeEvent(newEvent);
	}
	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		PowerZone.OnPowerChange -= OnPowerChange;
	}
}
