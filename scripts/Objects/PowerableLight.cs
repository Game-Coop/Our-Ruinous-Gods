using Godot;
using System;

public partial class PowerableLight : Node3D, IPower
{
    [Export] public int Charge { get; set; }
    [Export] public int Zone { get; set; }
    private Node3D light;
    [Export]public PowerState State { get; set; }

    public override void _Ready()
    {
        light = GetNode<Node3D>("Light3D");

        if(this.State == PowerState.On) {
            light.Show();
        } else {
            light.Hide();
        }

        EventBus EventBusHandler = GetNode<EventBus>("/root/EventBus");
        EventBusHandler.Connect("PowerEventHandler", new Callable(this, "OnPowerEvent"));
    }
    
    public void OnPowerEvent(int zone) {
        if(zone == this.Zone) {
            this.State = (this.State == PowerState.On) ? PowerState.Off : PowerState.On;
            
            if(this.State == PowerState.On) {
                light.Show();
            } else {
                light.Hide();
            }
            
            EventBus EventBusHandler = GetNode<EventBus>("/root/EventBus");

            PowerEvent newEvent = new PowerEvent();
            
            newEvent.Charge = this.Charge;
            newEvent.Zone = this.Zone;
            newEvent.State = this.State;
                 
            EventBusHandler.OnPowerChangeEvent(newEvent);
        }
    }
}
