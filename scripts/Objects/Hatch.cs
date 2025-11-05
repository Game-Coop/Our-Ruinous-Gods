using Godot;
using System;

public partial class Hatch : Node3D
{
    [Export] public bool useAnimation = false;
    [Export] public string openAnimationName = "hatch_open";
    [Export] public string closeAnimationName = "hatch_close";

    [Export] public Vector3 openPosition = new Vector3(0, 1, 0);
    [Export] public Vector3 closedPosition = new Vector3(0, 0, 0);
    [Export] public Vector3 openRotation = new Vector3(0, 0, 0);
    [Export] public Vector3 closedRotation = new Vector3(0, 0, 90);
    [Export] public float moveSpeed = 1f;

    private bool isOpen = false;
    private bool isMoving = false;

    private Vector3 targetPosition;
    private Vector3 targetRotation;

    private Area3D interactArea;
    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        if (useAnimation)
        {
            animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        }
        else
        {
            GlobalPosition = closedPosition;
            RotationDegrees = closedRotation;
        }

        interactArea = GetNode<Area3D>("InteractArea");
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("interact") && IsPlayerInRange() && !isMoving)
        {
            ToggleHatch();
        }

        if(!useAnimation && isMoving)
        {
            MoveAndRotateHatch(delta);
        }
    }

    private void ToggleHatch()
    {
        if (useAnimation)
        {
            if (animationPlayer.IsPlaying())
            {
                return;
            }

            if (isOpen)
            {
                animationPlayer.Play(closeAnimationName);
            }
            else
            {
                animationPlayer.Play(openAnimationName);
            }

            isOpen = !isOpen;
        }
        else
        {
            targetPosition = isOpen ? closedPosition : openPosition;
            targetRotation = isOpen ? closedRotation : openRotation;
            isMoving = true;
        }
    }

    private void MoveAndRotateHatch(double delta)
    {
        //Position
        Vector3 currentPosition = GlobalPosition;
        Vector3 direction = (targetPosition - currentPosition).Normalized();
        float distance = (targetPosition - currentPosition).Length();
        float moveStep = moveSpeed * (float)delta;

        //Rotation
        Vector3 currentRotation = RotationDegrees;
        Vector3 rotationDiff = targetRotation - currentRotation;
        float rotationStep = moveSpeed * 90f * (float)delta;

        bool positionDone = distance < 0.01f;
        bool rotationDone = rotationDiff.Length() < 0.5f;

        if (!positionDone)
        {
            GlobalPosition = currentPosition + direction * Math.Min(moveStep, distance);
        }

        if (!rotationDone)
        {
            RotationDegrees = currentRotation.Lerp(targetRotation, (float)(moveSpeed * delta));
        }

        if(positionDone && rotationDone)
        {
            GlobalPosition = targetPosition;
            RotationDegrees = targetRotation;
            isMoving = false;
            isOpen = !isOpen;
        }
    }

    private bool IsPlayerInRange()
    {
        var bodies = interactArea.GetOverlappingBodies();
        if(bodies == null) return false;

        foreach(var body in bodies)
        {
            if(body is Player)
            {
                return true;
            }
        }
        return false;
    }
}
