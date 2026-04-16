using Godot;
using System;

public interface IPuzzle : IInteractable
{
    public PuzzleData Data { get; }
    event Action<PuzzleData> OnSolve;
    event Action<PuzzleData> OnFail;
    event Action OnReset;
    event Action OnBack;
    void Input(Vector2 vector2);
    void Submit();
    void Back();
    void Reset();
}
