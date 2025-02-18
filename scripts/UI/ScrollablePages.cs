using System;
using System.Collections.Generic;
using Godot;

public class ScrollablePages : ScrollContainer
{
	private List<Page> pages = new List<Page>();
	[Export] private NodePath pageContainerPath;
	private BoxContainer pageContainer;
	private int separation;
	private bool isReady;
	private int targetScroll;
	public int PageCount => pages.Count;
	private int currentIndex;
	SceneTreeTween tween;
	public override void _Ready()
	{
		base._Ready();
		Init();
	}
	public void SelectPage(int toIndex, bool isInstant)
	{
		Init();
		pages[currentIndex].UnSelect();
		pages[toIndex].Select();
		AnimateScroll(toIndex, isInstant);
		currentIndex = toIndex;
	}
	public void ClearPages()
	{
		Init();
		currentIndex = 0;
		foreach (var page in pages)
		{
			page.QueueFree();
		}
		pages.Clear();
	}
	public Page AddPage(PackedScene pageTemplate)
	{
		Init();
		var page = pageTemplate.Instance() as Page;
		return AddPage(page);
	}
	public Page AddPage(Page page)
	{
		Init();
		GD.Print("Added page");
		pages.Add(page);
		pageContainer.AddChild(page);
		pageContainer.MoveChild(page, pageContainer.GetChildCount());
		return page;
	}
	public bool RemovePage(int pageIndex)
	{
		Init();
		if (pageIndex >= pages.Count) return false;
		var pageToRemove = pages[pageIndex];
		pageToRemove.QueueFree();
		return pages.Remove(pageToRemove);
	}
	private void Init()
	{
		if (isReady) return;
		isReady = true;

		pageContainer = GetNode<BoxContainer>(pageContainerPath);
		object seperationValue = pageContainer.Get("custom_constants/separation");
		separation = seperationValue == null ? 0 : (int)seperationValue;
		for (int i = 0; i < pageContainer.GetChildCount(); i++)
		{
			var page = pageContainer.GetChild(i) as Page;
			pages.Add(page);
		}
	}
	private void AnimateScroll(int pageIndex, bool isInstant)
	{
		if (ScrollVerticalEnabled)
		{
			targetScroll = (int)((RectSize.y + separation) * pageIndex);
			if (isInstant)
			{
				SetDeferred("scroll_vertical", targetScroll);
				return;
			}
		}
		else
		{
			targetScroll = (int)((RectSize.x + separation) * pageIndex);
			GD.Print("TargetScroll: " + targetScroll);
			if (isInstant)
			{
				SetDeferred("scroll_horizontal", targetScroll);
				return;
			}
		}

		if (tween != null && tween.IsValid())
		{
			tween.Kill();
		}
		tween = CreateTween();
		tween.TweenProperty(this, ScrollVerticalEnabled ? "scroll_vertical" : "scroll_horizontal", targetScroll, 0.3f)
		.FromCurrent()
		.SetTrans(Tween.TransitionType.Cubic)
		.SetEase(Tween.EaseType.Out);
	}
}
