
using System;
using Godot;

public partial class FadingPanelTweener : PanelTweener
{
    [Export] private NodePath foregroundPath;
    [Export] private float appearAlpha = 1f;
    [Export] private float disappearAlpha = 0f;
    [Export] private float appearTime = 0.3f;
    [Export] private float disappearTime = 0.3f;
    [Export] private Tween.EaseType appearEase = Tween.EaseType.Out;
    [Export] private Tween.TransitionType appearTransition = Tween.TransitionType.Back;
    [Export] private Tween.EaseType disappearEase = Tween.EaseType.Out;
    [Export] private Tween.TransitionType disappearTransition = Tween.TransitionType.Back;
    private Control _foreground;
    private Control foreground
    {
        get
        {
            if (_foreground == null)
            {
                _foreground = GetNode<Control>(foregroundPath);
            }
            return _foreground;
        }
    }
    public override void Appear(bool instant = false)
    {
        base.Appear(instant);

        foreground.Visible = true;
        if (instant)
        {
            var color = foreground.Modulate;
            color.a = 1f;
            foreground.Modulate = color;
            OnAppear();
            return;
        }
        tween = CreateTween();
        tween.TweenProperty(foreground, "modulate:a", appearAlpha, appearTime)
        .FromCurrent()
        .SetEase(appearEase)
        .SetTrans(appearTransition);
        tween.Connect("finished", new Callable(this, nameof(OnAppearComplete)));
    }
    public override void Disappear(bool instant = false)
    {
        base.Disappear(instant);
        if (instant)
        {
            var color = Modulate;
            color.a = 0f;
            Modulate = color;
            foreground.Visible = false;
            OnDisappear();
            return;
        }
        tween = CreateTween();
        tween.TweenProperty(foreground, "modulate:a", disappearAlpha, disappearTime)
        .FromCurrent()
        .SetEase(disappearEase)
        .SetTrans(disappearTransition);
        tween.Connect("finished", new Callable(this, nameof(OnDisappearComplete)));
    }
    private void OnDisappearComplete()
    {
        foreground.Visible = false;
        OnDisappear();
    }
    private void OnAppearComplete()
    {
        OnAppear();
    }

}