using Godot;
public class PowerEmitter : Spatial, IPower
{
    [Export] public int Charge { get; set; }
    public PowerState State { get; }
    [Export] public int Zone { get; set; }
    public override void _Ready()
    {
        EventBus EventBusHandler = GetNode<EventBus>("/root/EventBus");

        PowerEvent test = new PowerEvent();
        test.Charge = 101;
        test.State = PowerState.Off;
        test.Zone = 5;

        EventBusHandler.OnPowerChange(test);
        //EventBusHandler.OnWorldEvent("test world");
    }


    public void OnPowerChange(PowerEvent e)
	{
	}
}