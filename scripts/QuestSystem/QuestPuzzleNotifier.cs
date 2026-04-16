
using System;
using Godot;

public partial class QuestPuzzleNotifier : Node
{
    [Export] public NodePath puzzleNode;
    private IPuzzle puzzle;
    private EventBus eventBus;
    public override void _Ready()
    {
        base._Ready();
        eventBus = GetNode<EventBus>("/root/EventBus");
        puzzle = GetNode<IPuzzle>(puzzleNode);
       
        if (puzzle != null)
        {
            if (puzzle.Data.IsSolved)
            {
                OnSolve(puzzle.Data);
            }
            else
            {
                puzzle.OnSolve += OnSolve;
                puzzle.OnFail += OnFail;
            }
        }
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (puzzle != null)
        {
            puzzle.OnSolve -= OnSolve;
            puzzle.OnFail -= OnFail;
        }
    }
    private void OnSolve(PuzzleData data)
    {
        eventBus.OnQuestEvent(new QuestEvent($"puzzle_{data.Id}", true));
    }
    private void OnFail(PuzzleData data)
    {
        eventBus.OnQuestEvent(new QuestEvent($"puzzle_{data.Id}", false));
    }
}