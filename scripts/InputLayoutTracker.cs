using Godot;
using System;

public class InputLayoutTracker : Node
{
    public static InputLayout CurrentLayout { get; private set; } = InputLayout.KeyboardMouse;

    public static event Action<InputLayout> OnLayoutChanged;

    public override void _Input(InputEvent @event)
    {
        InputLayout newLayout = CurrentLayout;

        if (@event is InputEventJoypadButton || @event is InputEventJoypadMotion)
        {
            string name = Input.GetJoyName(@event.Device).ToLower();

            if (name.Contains("xbox"))
                newLayout = InputLayout.Xbox;
            else if (name.Contains("playstation") || name.Contains("ps4") || name.Contains("ps5") || name.Contains("dualshock"))
                newLayout = InputLayout.PlayStation;
            else
                newLayout = InputLayout.Xbox; // fallback for other controllers
        }
        else if (@event is InputEventKey || @event is InputEventMouse)
        {
            newLayout = InputLayout.KeyboardMouse;
        }

        if (newLayout != CurrentLayout)
        {
            CurrentLayout = newLayout;
            OnLayoutChanged?.Invoke(CurrentLayout);
        }
    }
}
