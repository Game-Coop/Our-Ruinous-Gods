using System.Collections.Generic;
using Godot;

public partial class OrderedPages : Control
{
	public int PageCount => pages.Count;
	private List<Page> pages = new List<Page>();
	[Export] private NodePath pageContainerPath;
	private Control pageContainer;
	private bool isReady;
	private int currentIndex;
	public override void _Ready()
	{
		base._Ready();
		Init();
	}
	public void SelectPage(int toIndex, bool isInstant)
	{
		Init();
		if (toIndex > currentIndex)
		{
			//forward
			// GD.Print("forward");
			pages[currentIndex].TweenerSetReverse(true);
			pages[toIndex].TweenerSetReverse(false);
			pages[currentIndex].HidePage(isInstant);
			pages[toIndex].ShowPage(isInstant);
		}
		else
		{
			//backward
			// GD.Print("backward");
			pages[toIndex].TweenerSetReverse(true);
			pages[currentIndex].TweenerSetReverse(false);
			pages[currentIndex].HidePage(isInstant);
			pages[toIndex].ShowPage(isInstant);
		}
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
		var page = pageTemplate.Instantiate() as Page;
		return AddPage(page);
	}
	public Page AddPage(Page page)
	{
		Init();
		// GD.Print("Added page");
		pages.Add(page);
		if (page.GetParent() != pageContainer)
		{
			pageContainer.AddChild(page);
		}
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

		pageContainer = GetNode<Control>(pageContainerPath);
		for (int i = 0; i < pageContainer.GetChildCount(); i++)
		{
			var page = pageContainer.GetChild(i) as Page;
			pages.Add(page);
		}
	}
}
