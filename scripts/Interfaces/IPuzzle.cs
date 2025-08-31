using System;
using Godot;

public interface IPuzzle : IInteractable
{
    event Action<BaseEventData> OnSolve;
    event Action<BaseEventData> OnFail;
    event Action<BaseEventData> OnReset;
    event Action<BaseEventData> OnBack;
    void Input(Vector2 vector2);
    void Submit();
    void Back();
    void Reset();
}
