using Godot;
using System;
using System.Numerics;

public partial class LiftTestPuzzle : Node3D, IPuzzle
{
    [Export] public string puzzleId = "lift_test_puzzle";
    private Area3D interactArea;
    private bool isSolved = false;

    public event Action<BaseEventData> OnSolve;
    public event Action<BaseEventData> OnFail;
    public event Action<BaseEventData> OnReset;

    public override void _Ready()
    {
        interactArea = GetNode<Area3D>("InteractArea");
    }

    public override void _Process(double delta)
    {
        if(isSolved) return;

        if(Input.IsActionJustPressed("interact") && IsPlayerInRange())
        {
            SolvePuzzle();
        }
    }

    private void SolvePuzzle()
    {
        isSolved = true;

        var eventBus = GetNode<EventBus>("EventBus");
        eventBus.OnPuzzleSolved(puzzleId);

        OnSolve?.Invoke(new PuzzleEvent(puzzleId, true);
        GD.Print($"TestPuzzle {puzzleId} solved!");
    }

    private bool IsPlayerInRange()
    {
        var bodies = interactArea.GetOverlappingBodies();
        if(bodies == null) return false;

        foreach (var body in bodies)
        {
            if(body is Player)
            {
                return true;
            }
        }
        return false;
    }

}
