
using System;
using System.Collections.Generic;
using Godot;

public class ControlsMenu : Page
{
	[Export] private PackedScene paginationTemplate;
	[Export] private NodePath navBarPath;
	[Export] private NodePath orderedPagesPath;
	[Export] private NodePath keyboardMenuPath;
	[Export] private NodePath gamepadMenuPath;
	private Navbar navbar;
	private OrderedPages orderedPages;
	private Page keyboardMenu;
	private Page gamepadMenu;
	protected override void _Ready()
	{
		base._Ready();
		navbar = GetNode<Navbar>(navBarPath);
		orderedPages = GetNode<OrderedPages>(orderedPagesPath);
		keyboardMenu = GetNode<Page>(keyboardMenuPath);
		gamepadMenu = GetNode<Page>(gamepadMenuPath);

		orderedPages.AddPage(keyboardMenu);
		orderedPages.AddPage(gamepadMenu);

		var paginationKeyboardMouse = navbar.AddPagination(paginationTemplate);
		var paginationGamePad = navbar.AddPagination(paginationTemplate);

		paginationKeyboardMouse.SetTitle("Keyboard & Mouse");
		paginationGamePad.SetTitle("Gamepad");
		navbar.OnNavigate += OnNavigate;

	}

	private void OnNavigate(int from, int to)
	{
		orderedPages.SelectPage(to, false);
	}

	public override void ShowPage(bool instant = false)
	{
		base.ShowPage(instant);
		orderedPages.SelectPage(0, true);
		navbar.NavigateWithoutNotify(0);
	}
	public override void HidePage(bool instant = false)
	{
		base.HidePage(instant);
	}
}
