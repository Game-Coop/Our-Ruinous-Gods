using System;

public interface ISwitchable
{
    event Action<bool> OnStateChange;
    bool IsOn { get; }
    void TurnOn();
    void TurnOff();
    void Toggle();
}