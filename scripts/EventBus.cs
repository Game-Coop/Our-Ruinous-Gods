using Godot;
public partial class EventBus : Node {
    [Signal] 
    public delegate void PowerChangedEventHandler(PowerEvent e);
    [Signal]
    public delegate void WorldEventHandler(WorldEvent e);
    public void OnPowerChange(PowerEvent e)
    {
        EmitSignal("PowerChangedEventHandler", e);
    }
    public void OnWorldEvent(WorldEvent e)
    {
        EmitSignal("WorldEventHandler", e);
    }
}