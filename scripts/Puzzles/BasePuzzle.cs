using System;
using Godot;
using PhantomCamera;

public partial class BasePuzzle : Interactable, IPuzzle
{
    public event Action<PuzzleData> OnSolve;
    public event Action<PuzzleData> OnFail;
    public event Action OnReset;
    public event Action OnBack;
    [Export] public PuzzleData Data { get; private set; }
    [Export] protected Node3D labels;

    [Export] Node3D phantomCamNode;

    protected PhantomCamera3D phantomCam;
    EventBus eventBus;
    public bool IsInteracting { get; private set; }
    public override void _Ready()
    {
        base._Ready();
        labels.Visible = false;
        phantomCam = phantomCamNode.AsPhantomCamera3D();
        eventBus = GetNode<EventBus>("/root/EventBus");
        phantomCam.TweenCompleted += () => labels.Visible = true;
    }

    public override bool CanInteract()
    {
        return base.CanInteract() && !IsInteracting && !Data.IsSolved;
    }
    /// <summary>
    /// Called when entering puzzle interaction
    /// </summary>
    public override void Interact()
    {
        base.Interact();
        IsInteracting = true;
        phantomCam.Priority = 100;
        eventBus.OnPuzzleInteract(new PuzzleInteractEvent(this));
        GameManager.Instance.Player.SetEnabled(false);
        GameManager.Instance.InWorldPuzzle = true;
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
        IsInteracting = false;
        phantomCam.Priority = 0;

        GameManager.Instance.Player.SetEnabled(true);
        GameManager.Instance.InWorldPuzzle = false;
        OnBack?.Invoke();
        labels.Visible = false;
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
            Data.IsSolved = true;
            OnSolve?.Invoke(Data);
            Back();
        }
        else
        {
            OnFail?.Invoke(Data);
        }
    }
}