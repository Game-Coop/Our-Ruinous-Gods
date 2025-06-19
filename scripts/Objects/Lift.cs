using Godot;
using System;

public partial class Lift : Node3D
{
    [Export] public string puzzleId = "lift_puzzle"; //ADD the actual puzzle ID here
    [Export] public string upAnimation = "lift_up";
    [Export] public string downAnimation = "lift_down";

    private bool isPuzzleSolved = false;
    private bool isUp = true;
    private AnimationPlayer animationPlayer;
    private Area3D interactArea;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        interactArea = GetNode<Area3D>("InteractArea");

        isUp = true; // Start with the lift in the up position

        EventBus eventBus = GetNode<EventBus>("/root/EventBus");
        eventBus.PuzzleSolved += OnPuzzleSolved;
    }

    private void OnPuzzleSolved(string puzzleId)
    {
        if (this.puzzleId == puzzleId)
        {
            isPuzzleSolved = true;
            GD.Print($"Puzzle {puzzleId} solved. Lift unlocked.");
        }
    }

    public override void _Process(double delta)
    {
        if (!isPuzzleSolved) return;

        if(Input.IsActionJustPressed("interact") && IsPlayerInRange())
        {
            ToggleLift();
        }
    }

    private void ToggleLift()
    {
        if(animationPlayer.IsPlaying()) return;

        if (isUp)
        {
            animationPlayer.Play(downAnimation);
        }
        else
        {
            animationPlayer.Play(upAnimation);
        }

        isUp = !isUp;
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
