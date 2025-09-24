using Godot;
using System;

public partial class LiftTestPuzzle : BasePuzzle
{
    private Area3D interactArea;

    public override void _Ready()
    {
        base._Ready();
        interactArea = GetNode<Area3D>("InteractArea");
    }

    public override void _Process(double delta)
    {
        if(Data.IsSolved) return;

        if(Godot.Input.IsActionJustPressed("interact") && IsPlayerInRange())
        {
            Interact();
        }
    }

    public override void Interact()
    {
        base.Interact();
        SolvePuzzle();
    }

    public override void Input(Vector2 vector2)
    {
        GD.Print($"Input received: {vector2}");
        // Here you can handle input from the player, e.g., for a joystick or touch input
    }

    public override void Submit()
    {
        GD.Print("Puzzle submitted");
        SolvePuzzle();
    }

    private void SolvePuzzle()
    {
        if (Data.IsSolved) return;

        SetSolved(true);
    }

    private bool IsPlayerInRange()
    {
        var bodies = interactArea.GetOverlappingBodies();
        if(bodies == null) return false;

        foreach (var body in bodies)
        {
            if (body is Player player)
            {
                return true;
            }
        }
        return false;
    }
}
