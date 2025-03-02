
using System;
using Godot;

public abstract class PanelTweener : Control
{
	public enum Direction
	{
		LeftToRight = 0,
		RightToLeft,
		UpToDown,
		DownToUp,
	}
	public event Action OnAppeared;
	public event Action OnDisappeared;
	public bool IsHidden { get; protected set; }
	[Export] public bool isHiddenAtStart = true;
	protected SceneTreeTween tween;
	protected bool isReverse;
	public bool IsReverse => isReverse;
	public override void _Ready()
	{
		base._Ready();
		if (isHiddenAtStart)
		{
			Disappear(true);
			GD.Print("disappear called on start");
		}
	}
	public virtual void Appear(bool instant = false)
	{
		if (IsHidden == false) return;
		IsHidden = false;
		KillTweens();
	}
	public virtual void Disappear(bool instant = false)
	{
		if (IsHidden) return;
		IsHidden = true;
		KillTweens();
	}
	protected void OnAppear()
	{
		OnAppeared?.Invoke();
		OnAppeared = null;
	}
	protected void OnDisappear()
	{
		OnDisappeared?.Invoke();
		OnDisappeared = null;
	}
	public virtual void Replicate(PanelTweener sourceTweener, bool isReverse)
	{
	}
	public virtual void RevertReplicate()
	{
	}
	protected virtual void KillTweens()
	{
		if (tween != null && tween.IsValid())
		{
			tween.Kill();
		}
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		KillTweens();
	}
}
