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

        if(IsPlayerInRange())
        {
            Interact();
        }
    }

    /*To be removed when puzzle is implemented
     * now used to test lift functionality
     * by setting the puzzle as solved when interacted with
     */
    public override void Interact()
    {
        base.Interact();
        SolvePuzzle();
    }


    public override void Submit()
    {
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
