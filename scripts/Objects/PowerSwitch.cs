using Godot;
using System;

public class PowerSwitch : Interactable
{
    [Export] public int Zone { get; set; }

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
	}
}
