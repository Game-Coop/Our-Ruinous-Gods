using Godot;
public class PowerEmitter : Spatial, IPower
{
    [Export] public int Charge { get; set; }
    public PowerState State { get; }
    [Export] public int Zone { get; set; }

    public override void _Ready()
    {
        GetNode<EventBus>("/root/EventBus").emit('WorldEventHandler');
    }

    public void OnPowerChange(PowerEvent e)
	{
	}
}