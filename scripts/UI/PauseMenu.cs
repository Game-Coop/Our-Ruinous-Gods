using System;
using Godot;

public partial class PauseMenu : Page
{
	[Export] private NodePath navBarPath;
	[Export] private NodePath orderedPagesPath;
	[Export] private NodePath inventoryPagePath;
	[Export] private NodePath journalPagePath;
	[Export] private NodePath audioPlayerPagePath;
	[Export] private PackedScene paginationTemplate;
	private Page inventoryPage;
	private Page journalPage;
	private Page audioPlayerPage;
	private Navbar navbar;
	private OrderedPages orderedPages;
	public override void _Ready()
	{
		base._Ready();
		navbar = GetNode<Navbar>(navBarPath);
		orderedPages = GetNode<OrderedPages>(orderedPagesPath);
		inventoryPage = GetNode<Page>(inventoryPagePath);
		journalPage = GetNode<Page>(journalPagePath);
		audioPlayerPage = GetNode<Page>(audioPlayerPagePath);

		orderedPages.AddPage(inventoryPage);
		orderedPages.AddPage(journalPage);
		orderedPages.AddPage(audioPlayerPage);

		var paginationInventory = navbar.AddPagination(paginationTemplate);
		var paginationJournal = navbar.AddPagination(paginationTemplate);
		var paginationAudioplayer = navbar.AddPagination(paginationTemplate);

		paginationInventory.SetTitle("Inventory");
		paginationJournal.SetTitle("Journal");
		paginationAudioplayer.SetTitle("Audio Player");

		navbar.OnNavigate += Navbar_OnNavigate;
	}
	public override void HidePage(bool instant = false)
	{
		base.HidePage(instant);
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	public override void ShowPage(bool instant = false)
	{
		base.ShowPage(instant);
		GetTree().Paused = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
	private void OpenPage(Page page, bool isInstant = false)
	{
		var destinationIndex = page.GetIndex();
		if (isInstant)
		{
			navbar.NavigateWithoutNotify(destinationIndex);
			orderedPages.SelectPage(destinationIndex, isInstant);
		}
		else
			navbar.NavigateTo(destinationIndex);
	}
	private void Navbar_OnNavigate(int from, int to)
	{
		orderedPages.SelectPage(to, false);
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (inventoryPage == null || journalPage == null || audioPlayerPage == null)
		{
			GD.PrintErr("Pages are not initialized properly.");
			return;
		}
		if (!Visible) return;

		if (@event.IsActionPressed("pause_toggle"))
		{
			HidePage();
			return;
		}
		var toggleInventory = @event.IsActionPressed("inventory_toggle");
		var toggleJournal = @event.IsActionPressed("journal_toggle");
		var toggleAudioplayer = @event.IsActionPressed("audioplayer_toggle");

		if (toggleInventory || toggleInventory || toggleAudioplayer)
			GetViewport().SetInputAsHandled();

		if (inventoryPage.Visible && toggleInventory)
		{
			HidePage();
		}
		else if (journalPage.Visible && toggleJournal)
		{
			HidePage();
		}
		else if (audioPlayerPage.Visible && toggleAudioplayer)
		{
			HidePage();
		}
		else if (!journalPage.Visible && toggleJournal)
		{
			OpenPage(journalPage, false);
		}
		else if (!inventoryPage.Visible && toggleInventory)
		{
			OpenPage(inventoryPage, false);
		}
		else if (!audioPlayerPage.Visible && toggleAudioplayer)
		{
			OpenPage(audioPlayerPage, false);
		}
	}
	public void OpenInventory()
	{
		if (Visible)
		{
			OpenPage(inventoryPage);
		}
		else
		{
			ShowPage();
			OpenPage(inventoryPage, true);
		}
	}

	public void OpenJournal()
	{
		if (Visible)
		{
			OpenPage(journalPage);
		}
		else
		{
			ShowPage();
			OpenPage(journalPage, true);
		}
	}

	public void OpenAudioPlayer()
	{
		if (Visible)
		{
			OpenPage(audioPlayerPage);
		}
		else
		{
			ShowPage();
			OpenPage(audioPlayerPage, true);
		}
	}
}
