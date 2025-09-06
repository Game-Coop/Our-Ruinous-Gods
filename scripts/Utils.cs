using Godot;
using System;

public partial class Utils : Node
{
    private static Utils _instance;

    public override void _EnterTree()
    {
        _instance = this;
    }

    public static async void DelayedCall(Action action, float delaySeconds)
    {
        await _instance.ToSignal(
            _instance.GetTree().CreateTimer(delaySeconds),
            SceneTreeTimer.SignalName.Timeout
        );

        action?.Invoke();
    }
}
