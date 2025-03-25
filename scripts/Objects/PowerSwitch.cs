using Godot;
using System;

public class PowerSwitch : Interactable, IPower
{
    [Export] public int Charge { get; set; }
    public PowerState State { get; set; }
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

        this.State = (this.State == PowerState.On) ? PowerState.Off : PowerState.On;
        
        PowerEvent e = new PowerEvent();
        
        e.Charge = this.Charge;
        e.State = this.State;
        e.Zone = this.Zone;
        
        EventBusHandler.OnPowerChange(e);
	}
}
