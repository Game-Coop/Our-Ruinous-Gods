using Godot;

public class PauseMenu : Control
{
	[Export] private NodePath navBarPath;
	[Export] private NodePath scrollablePagesPath;
	[Export] private PackedScene paginationTemplate;
	[Export] private PackedScene inventoryTemplate;
	[Export] private PackedScene journalTemplate;
	private Page inventoryPage;
	private Page journalPage;
	private Navbar navbar;
	private ScrollablePages scrollablePages;
	private bool isShown;

	private int GetIndex(Page page) => page.GetIndex(); 
	public override void _Ready()
	{
		base._Ready();
		navbar = GetNode<Navbar>(navBarPath);
		scrollablePages = GetNode<ScrollablePages>(scrollablePagesPath);
		inventoryPage = scrollablePages.AddPage(inventoryTemplate);
		journalPage = scrollablePages.AddPage(journalTemplate);
		var paginationInventory = navbar.AddPagination(paginationTemplate);
		var paginationJournal = navbar.AddPagination(paginationTemplate);
		paginationInventory.SetTitle("Inventory");
		paginationJournal.SetTitle("Journal");

		navbar.OnNavigate += Navbar_OnNavigate;
	}
	public override void _Input(InputEvent @event)
	{
		var toggleInventory = @event.IsActionPressed("inventory_toggle");
		var toggleJournal = @event.IsActionPressed("journal_toggle");

		if(toggleInventory || toggleInventory)
			GetTree().SetInputAsHandled();

		if (inventoryPage.IsSelected && toggleInventory && isShown)
		{
			SetVisibility(false);
		}
		else if(journalPage.IsSelected && toggleJournal && isShown)
		{
			SetVisibility(false);
		}
		else if(inventoryPage.IsSelected && toggleJournal && isShown)
		{
			OpenPage(journalPage, false);
		}
		else if(journalPage.IsSelected && toggleInventory && isShown)
		{
			OpenPage(inventoryPage, false);
		}
		else if(toggleInventory && isShown == false)
		{
			SetVisibility(true);
			OpenPage(inventoryPage);
		}
		else if (toggleJournal && isShown == false)
		{
			SetVisibility(true);
			OpenPage(journalPage);
		}
	}
	private void SetVisibility(bool value)
	{
		if (value == false)
		{
			isShown = false;
			Input.MouseMode = Input.MouseModeEnum.Captured;
			Hide();
		}
		else
		{
			isShown = true;
			Input.MouseMode = Input.MouseModeEnum.Visible;
			Show();
		}
	}
	private void OpenPage(Page page, bool isInstant = true)
	{
		var destinationIndex = GetIndex(page);
		if(isInstant)
			navbar.NavigateWithoutNotify(destinationIndex);
		else
			navbar.NavigateTo(destinationIndex);
		scrollablePages.SelectPage(destinationIndex, true);
	}
	private void Navbar_OnNavigate(int from, int to)
	{
		scrollablePages.SelectPage(to, false);
	}
}
