
using System;
using Godot;

public abstract partial class PanelTweener : Control
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
	protected Tween tween;
	protected bool isReverse;
	public bool IsReverse => isReverse;
	public override void _Ready()
	{
		base._Ready();
		if (isHiddenAtStart)
		{
			Disappear(true);
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
	public virtual void SetReverse(bool isReverse)
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
