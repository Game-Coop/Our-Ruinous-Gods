using System;
using Godot;

public partial class BasePuzzle : Interactable, IPuzzle
{
    public event Action<BaseEventData> OnSolve;
    public event Action<BaseEventData> OnFail;
    public event Action<BaseEventData> OnReset;
    public event Action<BaseEventData> OnBack;

    EventBus eventBus;

    /// <summary>
    /// Called when entering puzzle interaction
    /// </summary>
    public override void Interact()
    {
        base.Interact();
        eventBus = GetNode<EventBus>("/root/EventBus");
        eventBus.OnPuzzleInteract(new PuzzleInteractEvent(this));
        GameManager.Instance.Player.SetEnabled(false);
        HideHint();
    }

    /// <summary>
    /// Called when received wasd or arrows inputs
    /// </summary>
    /// <param name="vector2"></param>
    public virtual void Input(Vector2 vector2)
    {
    }

    /// <summary>
    /// Called when enter or ui button to accept
    /// </summary>
    public virtual void Submit()
    {
    }

    /// <summary>
    /// Called when exiting the puzzle interaction. Auto triggers itself when the puzzle solved. Otherwise
    /// it can be used to stop interacting with the puzzle.
    /// </summary>
    public virtual void Back()
    {
        GameManager.Instance.Player.SetEnabled(true);
        OnBack?.Invoke(null);
    }

    /// <summary>
    /// Called when reseting the puzzle.
    /// </summary>
    public virtual void Reset()
    {
        OnReset?.Invoke(null);
    }

    /// <summary>
    /// Should be called when the puzzle is completed or failed.
    /// </summary>
    /// <param name="isSolved"></param>
    protected void SetSolved(bool isSolved)
    {
        if (isSolved)
        {
            OnSolve?.Invoke(null);
            Back();
        }
        else
        {
            OnFail?.Invoke(null);
        }
    }
}