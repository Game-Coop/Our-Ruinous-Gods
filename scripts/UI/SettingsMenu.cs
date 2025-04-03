
using System;
using Godot;

public class SettingsMenu : Page
{
	[Export] private NodePath continueBtnPath;
	[Export] private NodePath controlsBtnPath;
	[Export] private NodePath gameplayBtnPath;
	[Export] private NodePath graphicsBtnPath;
	[Export] private NodePath exitBtnPath;
	[Export] private NodePath controlsMenuPath;
	[Export] private NodePath gameplayMenuPath;
	[Export] private NodePath graphicsMenuPath;
	private Button _continueBtn;
	private Button _exitBtn;
	private Button _controlsBtn;
	private Button _gameplayBtn;
	private Button _graphicsBtn;
	private Page controlsMenuPage;
	private Page gameplayMenuPage;
	private Page graphicsMenuPage;

	private Page currentPage;
	protected override void _Ready()
	{
		base._Ready();
		controlsMenuPage = GetNode<Page>(controlsMenuPath);
		gameplayMenuPage = GetNode<Page>(gameplayMenuPath);
		graphicsMenuPage = GetNode<Page>(graphicsMenuPath);

		_continueBtn = GetNode<Button>(continueBtnPath);
		_exitBtn = GetNode<Button>(exitBtnPath);
		
		_controlsBtn = GetNode<Button>(controlsBtnPath);
		_gameplayBtn = GetNode<Button>(gameplayBtnPath);
		_graphicsBtn = GetNode<Button>(graphicsBtnPath);

		_continueBtn.Connect("pressed", this, nameof(ContinuePressed));
		_exitBtn.Connect("pressed", this, nameof(OnExitPressed));

		_controlsBtn.Connect("pressed", this, nameof(ControlsPressed));
		_gameplayBtn.Connect("pressed", this, nameof(GameplayPressed));
		_graphicsBtn.Connect("pressed", this, nameof(GraphicsPressed));

		OnHidden += Hidden;
	}
	public override void ShowPage(bool instant = false)
	{
		base.ShowPage(instant);
		GetTree().Paused = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
	public override void HidePage(bool instant = false)
	{
		base.HidePage(instant);
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	private void ContinuePressed()
	{
		HidePage();
	}
	private void OnExitPressed()
	{
		// TODO: save and exit could be implemented here
		GetTree().ChangeSceneTo(ResourceDatabase.StartMenuScene);
	}
	private void ControlsPressed()
	{
		currentPage?.HidePage();

		currentPage = controlsMenuPage;

		if (!controlsMenuPage.Visible)
			controlsMenuPage.ShowPage();
		else
			controlsMenuPage.HidePage();
	}
	private void GameplayPressed()
	{
		currentPage?.HidePage();

		currentPage = gameplayMenuPage;

		if (!gameplayMenuPage.Visible)
			gameplayMenuPage.ShowPage();
		else
			gameplayMenuPage.HidePage();
	}
	private void GraphicsPressed()
	{
		currentPage?.HidePage();

		currentPage = graphicsMenuPage;

		if (!graphicsMenuPage.Visible)
			graphicsMenuPage.ShowPage();
		else
			graphicsMenuPage.HidePage();
	}
	private void Hidden()
	{
		if (currentPage != null && currentPage.Visible)
		{
			currentPage.HidePage(true);
		}
	}
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (!Visible) return;

		if (@event.IsActionPressed("settings_toggle"))
		{
			HidePage();
			GetTree().SetInputAsHandled();
		}
	}

	public void OpenSettings()
	{
		ShowPage();
	}
}
