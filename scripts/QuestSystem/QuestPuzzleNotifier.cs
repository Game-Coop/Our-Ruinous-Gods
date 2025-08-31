
using System;
using Godot;

public partial class QuestPuzzleNotifier : Node
{
    [Export] private string questVariable;
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
            puzzle.OnSolve += OnSolve;
            puzzle.OnFail += OnFail;
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
    private void OnSolve(BaseEventData data)
    {
        eventBus.OnQuestEvent(new QuestEvent(questVariable, true));
    }
    private void OnFail(BaseEventData data)
    {
        eventBus.OnQuestEvent(new QuestEvent(questVariable, false));
    }
}