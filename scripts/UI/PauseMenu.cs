using Godot;

public class PauseMenu : Control
{
	[Export] private NodePath navBarPath;
	[Export] private NodePath orderedPagesPath;
	[Export] private NodePath inventoryPagePath;
	[Export] private NodePath journalPagePath;
	[Export] private PackedScene paginationTemplate;
	// [Export] private PackedScene inventoryTemplate;
	// [Export] private PackedScene journalTemplate;
	private Page inventoryPage;
	private Page journalPage;
	private Navbar navbar;
	private OrderedPages orderedPages;
	private bool isShown;
	public override void _Ready()
	{
		base._Ready();
		navbar = GetNode<Navbar>(navBarPath);
		orderedPages = GetNode<OrderedPages>(orderedPagesPath);
		inventoryPage = GetNode<Page>(inventoryPagePath);
		journalPage = GetNode<Page>(journalPagePath);
		inventoryPage = orderedPages.AddPage(inventoryPage);
		journalPage = orderedPages.AddPage(journalPage);
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

		if (toggleInventory || toggleInventory)
			GetTree().SetInputAsHandled();

		if (inventoryPage.Visible && toggleInventory && isShown)
		{
			SetVisibility(false);
		}
		else if (journalPage.Visible && toggleJournal && isShown)
		{
			SetVisibility(false);
		}
		else if (inventoryPage.Visible && toggleJournal && isShown)
		{
			OpenPage(journalPage, false);
		}
		else if (journalPage.Visible && toggleInventory && isShown)
		{
			OpenPage(inventoryPage, false);
		}
		else if (toggleInventory && isShown == false)
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
}
