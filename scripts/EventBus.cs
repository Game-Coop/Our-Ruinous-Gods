using Godot;
public partial class EventBus : Node {
    [Signal] 
    public delegate void PowerEventHandler(int zone);
    [Signal] 
    public delegate void PowerChangedEventHandler(PowerEvent e);
    [Signal]
    public delegate void StaminaChangeEventHandler(int cost);
    [Signal]
    public delegate void WorldEventHandler(string name);
    public void OnPowerEvent(int zone)
    {
        EmitSignal("PowerEventHandler", zone);
    }
    public void OnPowerChangeEvent(PowerEvent e)
    {
        EmitSignal("PowerChangedEventHandler", e);
    }
    public void OnStaminaChangeEvent(int cost)
    {
        EmitSignal("StaminaChangeEventHandler", cost);
    }
    public void OnWorldEvent(string name)
    {
        EmitSignal("WorldEventHandler", name);
    }
}