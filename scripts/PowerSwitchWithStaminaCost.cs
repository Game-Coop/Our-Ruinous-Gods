using Godot;
using System;

public partial class PowerSwitchWithStaminaCost : Interactable
{
    [Export] public PowerZone PowerZone { get; private set; }
    [Export] public int Stamina { get; set; }

	public override string InteractionText => "Push";
	public override void _Ready()
	{
		base._Ready();
	}
	public override void Interact()
	{
		base.Interact();

        EventBus EventBusHandler = GetNode<EventBus>("/root/EventBus");
		if(PowerZone.TryTurnOn())
		{
        	EventBusHandler.OnStaminaChangeEvent(this.Stamina);
		}
	}
}
