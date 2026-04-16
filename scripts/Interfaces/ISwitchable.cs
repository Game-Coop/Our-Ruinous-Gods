using System;

public interface ISwitchable
{
    public event Action<bool> OnStateChange;
    public bool IsOn { get; }
    public bool CanTurnOn { get; }
    public void TurnOn();
    public void TurnOff();
    public void Toggle();
}