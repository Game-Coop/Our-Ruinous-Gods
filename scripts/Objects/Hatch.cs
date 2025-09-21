using Godot;
using System;

public partial class Hatch : Node3D
{
    [Export] public bool useAnimation = false;
    [Export] public string openAnimationName = "hatch_open";
    [Export] public string closeAnimationName = "hatch_close";

    [Export] public Vector3 openPosition = new Vector3(0, 1, 0);
    [Export] public Vector3 closedPosition = new Vector3(0, 0, 0);
    [Export] public float moveSpeed = 1f;

    private bool isOpen = false;
    private bool isMoving = false;
    private Vector3 targetPosition;
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
            MoveHatch(delta);
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
            isMoving = true;
        }
    }

    private void MoveHatch(double delta)
    {
        Vector3 currentPosition = GlobalPosition;
        Vector3 direction = (targetPosition - currentPosition).Normalized();
        float distance = (targetPosition - currentPosition).Length();
        float moveStep = moveSpeed * (float)delta;

        if(moveStep >= distance)
        {
            GlobalPosition = targetPosition;
            isMoving = false;
            isOpen = !isOpen;
        }
        else
        {
            GlobalPosition += direction * moveStep;
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
