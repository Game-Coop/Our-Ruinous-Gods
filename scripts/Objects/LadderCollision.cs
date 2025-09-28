using Godot;
using System;

public partial class LadderCollision : Area3D
{
    [Signal]
    public delegate void PlayerEnterLadderEventHandler();
    
    [Signal]
    public delegate void PlayerExitLadderEventHandler();

    public override void _Ready()
    {
        this.BodyEntered += OnBodyEntered;
        this.BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is Player player)
        {
            EmitSignal("PlayerEnterLadder");
        }
    }

    private void OnBodyExited(Node3D body)
    {
        if (body is Player player)
        {
            GD.Print("Player exited ladder detected in ladder script");
            EmitSignal("PlayerExitLadder");
        }
    }
}
