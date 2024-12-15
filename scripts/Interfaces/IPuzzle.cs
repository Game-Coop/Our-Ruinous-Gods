using System;
using System.Numerics;

public interface IPuzzle : IInteractable
{
    event Action<BaseEventData> OnSolve;
    event Action<BaseEventData> OnFail;
    event Action<BaseEventData> OnReset;
    void Input(Vector2 vector2);
    void Submit();
}
