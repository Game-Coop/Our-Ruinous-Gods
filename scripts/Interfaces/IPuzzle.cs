using Godot;
using System;

public interface IPuzzle : IInteractable
{
    event Action<PuzzleEvent> OnSolve;
    event Action<PuzzleEvent> OnFail;
    event Action<PuzzleEvent> OnReset;
    void Input(Vector2 vector2);
    void Submit();
}
