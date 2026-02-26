using Godot;
using System;

public enum PuzzleAudioTrigger
{
    Solved,
    Failed,
    Reset,
    Back
}

public partial class OnPuzzleEventAudio : AudioBehavior
{
    [Export] public PuzzleAudioTrigger Trigger;

    protected override void Setup()
    {
        var puzzle = FindParentOfType<IPuzzle>();
        if (puzzle == null)
        {
            return;
        }

        switch (Trigger)
        {
            case PuzzleAudioTrigger.Solved:
                puzzle.OnSolve += _ => Play(GetParent());
                break;
            case PuzzleAudioTrigger.Failed:
                puzzle.OnFail += _ => Play(GetParent());
                break;
            case PuzzleAudioTrigger.Reset:
                puzzle.OnReset += () => Play(GetParent());
                break;
            case PuzzleAudioTrigger.Back:
                puzzle.OnBack += () => Play(GetParent());
                break;
        }
    }
}
