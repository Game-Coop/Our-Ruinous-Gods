using System;
using Godot;

public partial class JournalEntryReader : Control
{
	[Export] private PackedScene paginationTemplate;
	[Export] private PackedScene pageTemplate;
	[Export] private NodePath navBarPath;
	[Export] private NodePath scrollablePagesPath;
	[Export] private string seperationString;

	private Navbar navbar;
	private ScrollablePages scrollablePages;
	public override void _Ready()
	{
		base._Ready();
		navbar = GetNode<Navbar>(navBarPath);
		scrollablePages = GetNode<ScrollablePages>(scrollablePagesPath);
		
		navbar.OnNavigate += Navbar_OnNavigate;
	}
	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		navbar.OnNavigate -= Navbar_OnNavigate;
    }

	private void Navbar_OnNavigate(int from, int to)
	{
		// GD.Print("Navigate");
		scrollablePages.SelectPage(to, false);
	}

	public void Setup(JournalData journalData)
	{
		SetupPages(journalData);
		SetupPaginations();
	}
	private void SetupPages(JournalData journalData)
	{
		var sentences = journalData.Content.Split(seperationString.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		scrollablePages.ClearPages();
		
		foreach (var sentence in sentences)
		{
			CreatePage(sentence);
		}
	}

	private void CreatePage(string text)
	{
		var page = scrollablePages.AddPage(pageTemplate);
		page.GetNode<Label>("Label").Text = text;
	}

	private void SetupPaginations()
	{
		navbar.Clear();
		for (int i = 0; i < scrollablePages.PageCount; i++)
		{
			navbar.AddPagination(paginationTemplate);
		}
	}
	
}
