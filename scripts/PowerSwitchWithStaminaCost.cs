using Godot;
using System;

public class PowerSwitchWithStaminaCost : Interactable
{
    [Export] public int Zone { get; set; }
    [Export] public int Stamina { get; set; }

	[Export] private NodePath switchableObjectPath;
	private ISwitchable switchable;
	public override string InteractionText => "Push";
	public override void _Ready()
	{
		base._Ready();
	}
	public override void Interact()
	{
		base.Interact();

        EventBus EventBusHandler = GetNode<EventBus>("/root/EventBus");        
        EventBusHandler.OnPowerEvent(this.Zone);   
        EventBusHandler.OnStaminaChangeEvent(this.Stamina);
	}
}
