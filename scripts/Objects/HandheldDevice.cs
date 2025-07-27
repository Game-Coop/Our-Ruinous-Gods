using System;
using Godot;

public partial class HandheldDevice : Node3D
{
    private double startPressTime;
    private bool holdingHandheldToggle = false;
    private Tween tween;
    [Export] private Vector3 offPosition;
    [Export] private Node3D focusNode;
    [Export] private Node3D unfocusNode;
    private double pressPoint = 0.1f;
    private double holdPoint = 1f;
    private bool _isFocused = false;
    private bool isFocused
    {
        get => _isFocused;
        set
        {
            if (_isFocused != value)
            {
                _isFocused = value;
                eventBus.OnHandheldFocused(isFocused);
            }
        }
    }
    private bool showedWithInput;
    EventBus eventBus;
    public override void _EnterTree()
    {
        base._EnterTree();
        //maybe add show handheld event here
    }
    public override void _ExitTree()
    {
        base._ExitTree();
    }
    public override void _Ready()
    {
        base._Ready();
        eventBus = GetNode<EventBus>("/root/EventBus");
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event.IsActionPressed("handheld_toggle"))
        {
            holdingHandheldToggle = true;
            startPressTime = GameTime.Time;
            if (!Visible)
            {
                showedWithInput = true;
                ShowHandheld();
            }
        }
        else if (@event.IsActionReleased("handheld_toggle"))
        {
            holdingHandheldToggle = false;
            var delta = GameTime.Time - startPressTime;
            if (delta >= pressPoint && delta < holdPoint)
            {
                if (Visible && !showedWithInput)
                {
                    HideHandheld();
                }
            }
            showedWithInput = false;
        }
    }
    private void ShowHandheld()
    {
        KillTween();

        Visible = true;
        tween = CreateTween();
        tween.SetParallel(true);

        tween.TweenProperty(this, "position", unfocusNode.Position, 0.3f)
        .FromCurrent()
        .SetEase(Tween.EaseType.Out)
        .SetTrans(Tween.TransitionType.Back);

        tween.TweenProperty(this, "rotation", unfocusNode.Rotation, 0.3f)
        .FromCurrent()
        .SetEase(Tween.EaseType.Out)
        .SetTrans(Tween.TransitionType.Back);

        tween.Finished += OnShowed;
    }
    private void OnShowed()
    {

    }
    private void HideHandheld()
    {
        KillTween();

        tween = CreateTween();
        tween.TweenProperty(this, "position", offPosition, 0.3f)
        .FromCurrent()
        .SetEase(Tween.EaseType.Out)
        .SetTrans(Tween.TransitionType.Back);
        tween.Finished += OnHidden;

        isFocused = false;
    }
    private void OnHidden()
    {
        Visible = false;
    }

    private void KillTween()
    {
        if (tween != null && tween.IsValid())
        {
            tween.Kill();
        }
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (holdingHandheldToggle)
        {
            var deltaTime = GameTime.Time - startPressTime;

            if (deltaTime > holdPoint)
            {
                holdingHandheldToggle = false;
                if (isFocused)
                {
                    isFocused = false;
                    UnfocusHandheld();
                }
                else
                {
                    isFocused = true;
                    FocusHandheld();
                }
            }
        }
    }
    private void FocusHandheld()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        GameManager.Instance.FocusedHandheld = true;

        KillTween();

        tween = CreateTween();
        tween.SetParallel(true);

        tween.TweenProperty(this, "position", focusNode.Position, 0.3f)
        .FromCurrent()
        .SetEase(Tween.EaseType.Out)
        .SetTrans(Tween.TransitionType.Sine);

        tween.TweenProperty(this, "rotation", focusNode.Rotation, 0.3f)
       .FromCurrent()
       .SetEase(Tween.EaseType.Out)
       .SetTrans(Tween.TransitionType.Sine);
    }

    private void UnfocusHandheld()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        GameManager.Instance.FocusedHandheld = false;

        KillTween();

        tween = CreateTween();
        tween.SetParallel(true);

        tween.TweenProperty(this, "position", unfocusNode.Position, 0.3f)
        .FromCurrent()
        .SetEase(Tween.EaseType.Out)
        .SetTrans(Tween.TransitionType.Sine);

        tween.TweenProperty(this, "rotation", unfocusNode.Rotation, 0.3f)
       .FromCurrent()
       .SetEase(Tween.EaseType.Out)
       .SetTrans(Tween.TransitionType.Sine);

    }
}