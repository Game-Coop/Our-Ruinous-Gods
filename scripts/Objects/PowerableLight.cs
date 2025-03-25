using Godot;
using System;

public class PowerableLight : Spatial, IPowerable
{
    [Export] public int Zone { get; set; }
    private Spatial light;

    public override void _Ready()
    {
        light = GetNode<Spatial>("Light");

        EventBus EventBusHandler = GetNode<EventBus>("/root/EventBus");
        EventBusHandler.Connect("PowerChangedEventHandler", this, "OnPowerChange");
    }
    public void OnPowerChange(PowerEvent e) {
        if(e.Zone == this.Zone) {
            if(e.State == PowerState.On) {
                light.Show();
            } else {
                light.Hide();
            }
        }
    }
}
