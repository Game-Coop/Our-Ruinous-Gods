using Godot;
using System;

public partial class PowerSwitch : Interactable, IPower
{
	public PowerZone PowerZone { get; private set; }
	public override string InteractionText => "Push";
	public override void _Ready()
	{
		base._Ready();
	}
	public override void Interact()
	{
		base.Interact();
		if (PowerZone.State == PowerState.On)
		{
			PowerZone.TurnOff();
		}
		else
		{
			if (PowerZone.TryTurnOn())
			{
				GD.Print("Power zone activated");
			}
			else
			{
				GD.Print("Failed to activate power zone");
			}
		}
	}
	public void Register(PowerZone powerZone)
	{
		PowerZone = powerZone;
	}
}
