using Godot;
using System;

public partial class LiftTestPuzzle : Node3D, IPuzzle
{
    [Export] public string puzzleId = "lift_test_puzzle";
    private Area3D interactArea;
    private bool isSolved = false;

    //IPuzzle events
    public event Action<BaseEventData> OnSolve;
    public event Action<BaseEventData> OnFail;
    public event Action<BaseEventData> OnReset;

    //IInteractable events
    public event Action<BaseEventData> OnInteract;

    //Assing the interaction text for the puzzle
    public string InteractionText => isSolved ? "Lift unlocked" : "Press E to solve the puzzle";

    public override void _Ready()
    {
        interactArea = GetNode<Area3D>("InteractArea");
    }

    public override void _Process(double delta)
    {
        if(isSolved) return;

        if(Godot.Input.IsActionJustPressed("interact") && IsPlayerInRange())
        {
            Interact();
        }
    }

    public void Interact()
    {
        OnInteract?.Invoke(new PuzzleEvent(puzzleId, isSolved));
        SolvePuzzle();
    }

    public void ShowHint()
    {
        GD.Print("Show hint for the lift puzzle");
    }

    public void HideHint()
    {
        GD.Print("Hide hint for the lift puzzle");
    }

    public void Input(Vector2 vector2)
    {
        GD.Print($"Input received: {vector2}");
        // Here you can handle input from the player, e.g., for a joystick or touch input
    }

    public void Submit()
    {
        GD.Print("Puzzle submitted");
        SolvePuzzle();
    }

    private void SolvePuzzle()
    {
        if (isSolved) return;

        isSolved = true;

        var eventBus = GetNode<EventBus>("/root/EventBus");
        eventBus.OnPuzzleSolvedEvent(puzzleId);

        OnSolve?.Invoke(new PuzzleEvent(puzzleId, true));
        GD.Print($"Testpuzzle '{puzzleId}' solved!");
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
