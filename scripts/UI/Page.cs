

using System;
using Godot;

public partial class Page : Control
{
	[Export] private NodePath panelTweenerPath;
	public event Action OnShow;
	public event Action OnHide;
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
	public override void _Ready()
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
		OnShow?.Invoke();

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
		OnHide?.Invoke();
		
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
	public void TweenerSetReverse(bool isReverse)
	{
		panelTweener.SetReverse(isReverse);
	}
}
