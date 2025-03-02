

using System;
using Godot;

public class Page : Control
{
	[Export] private NodePath panelTweenerPath;
	public event Action OnShown;
	public event Action OnHidden;
	private PanelTweener _panelTweener;
	private PanelTweener panelTweener
	{
		get
		{
			if (_panelTweener == null && panelTweenerPath != null)
			{
				_panelTweener = GetNode<PanelTweener>(panelTweenerPath);
			}
			return _panelTweener;
		}
	}
	protected virtual void _Ready()
	{
		base._Ready();
		OnHidden += Hide;
		if (panelTweener != null)
		{
			if (panelTweener.IsHidden)
			{
				OnHidden?.Invoke();
			}
		}
	}
	public virtual void ShowPage(bool instant = false)
	{
		Show();

		if (panelTweener == null)
		{
			OnShown?.Invoke();
		}
		else
		{
			panelTweener.OnAppeared += OnShown;
			panelTweener.Appear(instant);
		}
	}
	public virtual void HidePage(bool instant = false)
	{
		if (panelTweener == null)
		{
			OnHidden?.Invoke();
		}
		else
		{
			panelTweener.OnDisappeared += OnHidden;
			panelTweener.Disappear(instant);
		}
	}
	public void ReplicatePage(Page page, bool isReverse)
	{
		panelTweener.Replicate(page.panelTweener, isReverse);
	}
	public void RevertReplicate()
	{
		panelTweener.RevertReplicate();
	}
}
