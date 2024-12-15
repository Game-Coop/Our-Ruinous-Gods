
using System;
using Godot;

public class Lamp : Spatial, ISwitchable
{
    public event Action<bool> OnStateChange;
    public bool IsOn { get; private set; }

    public void Toggle()
    {
        if(IsOn) TurnOff();
        else TurnOn();
    }

    public void TurnOff()
    {
        GD.Print("Light turned off!");
        OnStateChange?.Invoke(false);
        IsOn = false;
    }

    public void TurnOn()
    {
        GD.Print("Light turned on!");
        OnStateChange?.Invoke(true);
        IsOn = true;
    }
}