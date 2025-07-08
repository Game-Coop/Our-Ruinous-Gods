using Godot;
public partial class EventBus : Node
{
	[Signal]
	public delegate void PowerEventHandler(int zone);
	[Signal]
	public delegate void PowerChangedEventHandler(PowerEvent e);
	[Signal]
	public delegate void StaminaChangeEventHandler(int cost);
	[Signal]
	public delegate void WorldEventHandler(string name);
	[Signal]
	public delegate void QuestEventHandler(string name, Variant value);
	public void OnPowerEvent(int zone)
	{
		EmitSignal(SignalName.Power, zone);
	}
	public void OnPowerChangeEvent(PowerEvent e)
	{
		EmitSignal(SignalName.PowerChanged, e);
	}
	public void OnStaminaChangeEvent(int cost)
	{
		EmitSignal(SignalName.StaminaChange, cost);
	}
	public void OnWorldEvent(string name)
	{
		EmitSignal(SignalName.World, name);
	}
	public void OnQuestEvent(string questVariable, Variant value)
	{
		EmitSignal(SignalName.Quest, questVariable, value);
	}
}
