using System;
using Godot;

public partial class PuzzleController : Node
{
    EventBus eventBus;
    private IPuzzle currentPuzzle;
    public override void _EnterTree()
    {
        base._EnterTree();
        eventBus = GetNode<EventBus>("/root/EventBus");
        eventBus.PuzzleInteract += OnPuzzleInteract;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        eventBus.PuzzleInteract -= OnPuzzleInteract;
    }
    public override void _Input(InputEvent @event)
    {
        if (currentPuzzle == null) return;

        if (@event.IsAction("move_forward") || @event.IsAction("move_backward") || @event.IsAction("move_left") || @event.IsAction("move_right"))
        {
            currentPuzzle.Input(Input.GetVector("move_left", "move_right", "move_forward", "move_backward"));
        }
        else if (@event.IsAction("submit"))
        {
            currentPuzzle.Submit();
        }
        else if (@event.IsActionPressed("back"))
        {
            currentPuzzle.Back();
        }
        else if (@event.IsActionPressed("reset"))
        {
            currentPuzzle.Reset();
        }
    }

    private void OnPuzzleInteract(PuzzleInteractEvent puzzleInteractEvent)
    {
        currentPuzzle = puzzleInteractEvent.puzzle;
        currentPuzzle.OnBack += CurrentPuzzleBack;
    }

    private void CurrentPuzzleBack(BaseEventData data)
    {
        currentPuzzle.OnBack -= CurrentPuzzleBack;
        currentPuzzle = null;
    }
}