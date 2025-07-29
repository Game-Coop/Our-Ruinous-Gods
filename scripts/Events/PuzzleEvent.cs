using Godot;
using System;

public partial class PuzzleEvent : BaseEventData
{
    public string PuzzleId { get; }
    public bool IsSolved { get; }

    public PuzzleEvent(string puzzleId, bool isSolved)
    {
        PuzzleId = puzzleId;
        IsSolved = isSolved;
    }
}
