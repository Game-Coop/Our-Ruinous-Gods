using Godot;
using System;

public class LadderCollision : Area
{
    [Signal]
    public delegate void PlayerEnterLadder();

    [Signal]
    public delegate void PlayerExitLadder();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("body_entered", this, nameof(OnBodyEntered));
        Connect("body_exited", this, nameof(OnBodyExited));
    }

    private void OnBodyEntered(Node body)
    {
        if (body is player)
        {
            EmitSignal(nameof(PlayerEnterLadder));
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is player)
        {
            EmitSignal(nameof(PlayerExitLadder));
        }
    }
}
