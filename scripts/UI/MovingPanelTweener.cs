
using System;
using System.Collections.Generic;
using Godot;

public class MovingPanelTweener : PanelTweener
{
	[Export] private NodePath foregroundPath;
	[Export] private Direction direction;
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
	private Dictionary<Direction, Func<Vector2>> disappearPos = new Dictionary<Direction, Func<Vector2>>();
	private Vector2 appearPos = Vector2.Zero;
	private bool isReady = false;
	private Direction originalDirection;
	private void SetupPositions()
	{
		if (isReady) return;
		isReady = true;

		float screenWidth = GetViewport().Size.x;
		float screenHeight = GetViewport().Size.y;
		float scaleFactor = 1;

		disappearPos.Add(Direction.LeftToRight, () => new Vector2(-screenWidth / scaleFactor * (isReverse ? -1f : 1f), 0));
		disappearPos.Add(Direction.RightToLeft, () => new Vector2(screenWidth / scaleFactor * (isReverse ? -1f : 1f), 0));
		disappearPos.Add(Direction.UpToDown, () => new Vector2(0, screenHeight / scaleFactor * (isReverse ? -1f : 1f)));
		disappearPos.Add(Direction.DownToUp, () => new Vector2(0, -screenHeight / scaleFactor * (isReverse ? -1f : 1f)));
		originalDirection = direction;
	}
	public override void Appear(bool instant = false)
	{
		base.Appear(instant);
		SetupPositions();

		foreground.Visible = true;
		if (instant)
		{
			var color = Modulate;
			color.a = 1f;
			Modulate = color;
			foreground.RectPosition = appearPos;
			OnAppear();
			return;
		}
		tween = CreateTween();
		// tween.TweenProperty(foreground, "modulate:a", appearAlpha, appearTime)
		// .FromCurrent()
		// .SetEase(appearEase)
		// .SetTrans(appearTransition);

		tween.TweenProperty(foreground, "rect_position", appearPos, appearTime)
		.FromCurrent()
		.SetEase(appearEase)
		.SetTrans(appearTransition);

		tween.Connect("finished", this, nameof(OnAppearComplete));
	}
	public override void Disappear(bool instant = false)
	{
		base.Disappear(instant);
		SetupPositions();

		if (instant)
		{
			var color = Modulate;
			color.a = 0f;
			Modulate = color;
			foreground.Visible = false;
			foreground.RectPosition = disappearPos[direction]();
			OnDisappear();
			return;
		}
		tween = CreateTween();
		// tween.TweenProperty(foreground, "modulate:a", disappearAlpha, disappearTime)
		// .FromCurrent()
		// .SetEase(disappearEase)
		// .SetTrans(disappearTransition);

		tween.TweenProperty(foreground, "rect_position", disappearPos[direction](), disappearTime)
		.FromCurrent()
		.SetEase(disappearEase)
		.SetTrans(disappearTransition);

		tween.Connect("finished", this, nameof(OnDisappearComplete));
	}
	public override void Replicate(PanelTweener sourceTweener, bool isReverse)
	{
		base.Replicate(sourceTweener, isReverse);
		if (sourceTweener is MovingPanelTweener tweener)
		{
			direction = tweener.direction;
			GD.Print("replicate called");
			this.isReverse = isReverse ? !sourceTweener.IsReverse : this.isReverse;
		}
		else
		{
			GD.PrintErr("Tweeners are of different type! Can't replicate");
		}
	}
	public override void RevertReplicate()
	{
		base.RevertReplicate();
		direction = originalDirection;
		isReverse = false;
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
