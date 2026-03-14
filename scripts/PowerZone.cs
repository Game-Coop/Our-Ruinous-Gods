using System;
using Godot;

public partial class PowerZone : Node, ISavable<SaveData>
{
    public PowerGrid powerGrid;
    [Export] public int Charge { get; private set; }
    public event Action<PowerZone> OnPowerChange;
    public PowerState _state;
    public PowerState State
    {
        get => _state;
        set
        {
            if(_state != value)
            {
                _state = value;
                OnPowerChange?.Invoke(this);
            }
        }
    }

    public override void _Ready()
    {
        base._Ready();
        powerGrid = GetParent<PowerGrid>();
        GD.Print("zone ready with index and state", GetIndex(), State, " ",GetInstanceId());
        powerGrid.Register(this);
        this.Traverse<IPower>((powerable) =>
        {
            powerable.Register(this);
        });
    }
    public bool TryTurnOn()
    {
        if (powerGrid.CurrentCharge + Charge <= powerGrid.MaxCharge)
        {
            State = PowerState.On;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void TurnOff()
    {
        State = PowerState.Off;
    }

    public void OnSave(SaveData data)
    {
        var index = GetIndex();
        if (data.powerZoneStates.ContainsKey(index))
        {
            data.powerZoneStates[index] = State;
        }
        else
        {
            data.powerZoneStates.Add(index, State);
        }
    }

    public void OnLoad(SaveData data)
    {
        var index = GetIndex();
        GD.Print("zone loaded with index", index, " ", GetInstanceId());
        if (data.powerZoneStates.TryGetValue(index, out var isOn))
        {
            State = isOn;
            GD.Print("zone loaded with state", isOn);
        }
    }
}
