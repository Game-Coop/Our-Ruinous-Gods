
using System;
using Godot;

public partial class SlidingDoor : Door
{
    [Export] private NodePath rightDoorPath;
    [Export] private NodePath leftDoorPath;
    [Export] private Tween.EaseType doorEase;
    [Export] private Tween.TransitionType easeTransition;
    [Export] private float duration = 0.5f;
    [Export] private Vector3 leftLocalOpenPos = new Vector3(-1f, 0, 0);
    [Export] private Vector3 leftLocalClosePos = new Vector3(0, 0, 0);
    [Export] private Vector3 rightLocalOpenPos = new Vector3(1f, 0, 0);
    [Export] private Vector3 rightLocalClosePos = new Vector3(0, 0, 0);

    private Node3D rightDoor;
    private Node3D leftDoor;
    protected Tween tween;
    public override void _Ready()
    {
        base._Ready();
        rightDoor = GetNode<Node3D>(rightDoorPath);
        leftDoor = GetNode<Node3D>(leftDoorPath);
    }
    public override void TurnOn()
    {
        base.TurnOn();

        if (tween != null) tween.Kill();

        tween = CreateTween();
        tween.SetParallel();

        tween.TweenProperty(rightDoor, "translation", rightLocalOpenPos, duration)
         .FromCurrent()
         .SetEase(doorEase)
         .SetTrans(easeTransition);

        tween.TweenProperty(leftDoor, "translation", leftLocalOpenPos, duration)
         .FromCurrent()
         .SetEase(doorEase)
         .SetTrans(easeTransition);
    }
    public override void TurnOff()
    {
        base.TurnOff();

        if (tween != null) tween.Kill();

        tween = CreateTween();
        tween.SetParallel();

        tween.TweenProperty(rightDoor, "translation", rightLocalClosePos, duration)
         .FromCurrent()
         .SetEase(doorEase)
         .SetTrans(easeTransition);

        tween.TweenProperty(leftDoor, "translation", leftLocalClosePos, duration)
         .FromCurrent()
         .SetEase(doorEase)
         .SetTrans(easeTransition);
    }
}