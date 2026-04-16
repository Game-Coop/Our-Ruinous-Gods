using Godot;
using System;

public partial class Lift : Node3D
{
    [Export] public PuzzleData RequiredPuzzle;
    /*[Export] public string upAnimation = "lift_up";
    [Export] public string downAnimation = "lift_down";*/

    [Export] public Vector3 upPosition = new Vector3(0, 5, 0);
    [Export] public Vector3 downPosition = new Vector3(0, 0, 0);
    [Export] public float moveSpeed = 3f;

    private bool isPuzzleSolved = false;
    private bool isUp = true;
    private bool isMoving = false;

    private AnimationPlayer animationPlayer;
    private Vector3 targetPosition;
    private Area3D interactArea;

    public override void _Ready()
    {
        //animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        GlobalPosition = upPosition;
        interactArea = GetNode<Area3D>("InteractArea");

        if(RequiredPuzzle != null)
        {
            var eventBus = GetNode<EventBus>("/root/EventBus");
            eventBus.PuzzleSolved += OnPuzzleSolved;

            if (RequiredPuzzle.IsSolved)
            {
                UnlockLift();
            }
        }
        else
        {
            GD.PushError("Lift has no PuzzleData assigned!");
        }
    }

    private void OnPuzzleSolved(PuzzleData solvedPuzzle)
    {
        if (solvedPuzzle == RequiredPuzzle)
        {
            UnlockLift();
        }
    }

    private void UnlockLift()
    {
        isPuzzleSolved = true;
    }

    public override void _Process(double delta)
    {
        if (!isPuzzleSolved) return;

        if (Input.IsActionJustPressed("interact") && IsPlayerInRange() && !isMoving)
        {
            ToggleLift();
        }

        if (isMoving)
        {
            MoveLift(delta);
        }
    }

    private void ToggleLift()
    {
        /*if (animationPlayer.IsPlaying()) return;

        if (isUp)
        {
            animationPlayer.Play(downAnimation);
        }
        else
        {
            animationPlayer.Play(upAnimation);
        }

        isUp = !isUp;*/

        targetPosition = isUp ? downPosition : upPosition;
        isMoving = true;
    }

    private void MoveLift(double delta)
    {
        Vector3 currentPosition = GlobalPosition;
        Vector3 direction = (targetPosition - currentPosition).Normalized();
        float distance = (targetPosition - currentPosition).Length();
        float moveStep = moveSpeed * (float)delta;

        if (moveStep >= distance)
        {
            GlobalPosition = targetPosition;
            isUp = !isUp;
            isMoving = false;
        }
        else
        {
            GlobalPosition += direction * moveStep;
        }
    }

    private bool IsPlayerInRange()
    {
        var bodies = interactArea.GetOverlappingBodies();
        if (bodies == null) return false;

        foreach (var body in bodies)
        {
            if (body is Player player)
            {
                return true;
            }
        }
        return false;
    }
}