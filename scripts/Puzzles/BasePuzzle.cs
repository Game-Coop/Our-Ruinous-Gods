using System;
using Godot;

public partial class BasePuzzle : Interactable, IPuzzle
{
    [Export] public PuzzleData puzzleData;
    public event Action<PuzzleData> OnSolve;
    public event Action<PuzzleData> OnFail;
    public event Action OnReset;
    public event Action OnBack;

    EventBus eventBus;

    public override bool CanInteract()
    {
        return !puzzleData.IsSolved;
    }
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
        OnBack?.Invoke();
    }

    /// <summary>
    /// Called when reseting the puzzle.
    /// </summary>
    public virtual void Reset()
    {
        OnReset?.Invoke();
    }

    /// <summary>
    /// Should be called when the puzzle is completed or failed.
    /// </summary>
    /// <param name="isSolved"></param>
    protected void SetSolved(bool isSolved)
    {
        if (isSolved)
        {
            puzzleData.IsSolved = true;
            OnSolve?.Invoke(puzzleData);
            Back();
        }
        else
        {
            OnFail?.Invoke(puzzleData);
        }
    }
}