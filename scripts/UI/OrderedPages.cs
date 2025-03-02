using System.Collections.Generic;
using Godot;

public class OrderedPages : Control
{
	private List<Page> pages = new List<Page>();
	[Export] private NodePath pageContainerPath;
	private Control pageContainer;
	private bool isReady;
	public int PageCount => pages.Count;
	private int currentIndex;
	public override void _Ready()
	{
		base._Ready();
		Init();
	}
	public void SelectPage(int toIndex, bool isInstant)
	{
		Init();
		pages[currentIndex].ReplicatePage(pages[toIndex], true);
		pages[currentIndex].HidePage(isInstant);
		pages[toIndex].ShowPage(isInstant);
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
		if(page.GetParent() != pageContainer)
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
