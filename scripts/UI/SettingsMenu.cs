using Godot;

public partial class SettingsMenu : Page
{
	[Export] private NodePath continueBtnPath;
	[Export] private NodePath controlsBtnPath;
	[Export] private NodePath gameplayBtnPath;
	[Export] private NodePath graphicsBtnPath;
	[Export] private NodePath audioBtnPath;
	[Export] private NodePath exitBtnPath;
	[Export] private NodePath controlsMenuPath;
	[Export] private NodePath gameplayMenuPath;
	[Export] private NodePath graphicsMenuPath;
	[Export] private NodePath audioMenuPath;
	private Button _continueBtn;
	private Button _exitBtn;
	private Button _controlsBtn;
	private Button _gameplayBtn;
	private Button _graphicsBtn;
	private Button _audioBtn;
	private Page controlsMenuPage;
	private Page gameplayMenuPage;
	private Page graphicsMenuPage;
	private Page audioMenuPage;

	private Page currentPage;
	public override void _Ready()
	{
		base._Ready();
		controlsMenuPage = GetNode<Page>(controlsMenuPath);
		gameplayMenuPage = GetNode<Page>(gameplayMenuPath);
		graphicsMenuPage = GetNode<Page>(graphicsMenuPath);
		audioMenuPage = GetNode<Page>(audioMenuPath);

		_continueBtn = GetNode<Button>(continueBtnPath);
		_exitBtn = GetNode<Button>(exitBtnPath);

		_controlsBtn = GetNode<Button>(controlsBtnPath);
		_gameplayBtn = GetNode<Button>(gameplayBtnPath);
		_graphicsBtn = GetNode<Button>(graphicsBtnPath);
		_audioBtn = GetNode<Button>(audioBtnPath);

		_continueBtn.Connect("pressed", new Callable(this, nameof(ContinuePressed)));
		_exitBtn.Connect("pressed", new Callable(this, nameof(OnExitPressed)));

		_controlsBtn.Connect("pressed", new Callable(this, nameof(ControlsPressed)));
		_gameplayBtn.Connect("pressed", new Callable(this, nameof(GameplayPressed)));
		_graphicsBtn.Connect("pressed", new Callable(this, nameof(GraphicsPressed)));
		_audioBtn.Connect("pressed", new Callable(this, nameof(AudioPressed)));

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
		GetTree().ChangeSceneToPacked(ResourceDatabase.StartMenuScene);
	}
	private void ControlsPressed()
	{
		SelectPage(controlsMenuPage);
	}
	private void GameplayPressed()
	{
		SelectPage(gameplayMenuPage);
	}
	private void GraphicsPressed()
	{
		SelectPage(graphicsMenuPage);
	}
	private void AudioPressed()
	{
		SelectPage(audioMenuPage);
	}
	private new void Hidden()
	{
		if (currentPage != null && currentPage.Visible)
		{
			currentPage.HidePage(true);
		}
	}
	private void SelectPage(Page page)
	{
		currentPage?.HidePage();

		currentPage = page;

		if (!page.Visible)
			page.ShowPage();
		else
			page.HidePage();
	}
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (!Visible) return;

		if (@event.IsActionPressed("pause_toggle"))
		{
			HidePage();
			GetViewport().SetInputAsHandled();
		}
	}

	public void OpenSettings()
	{
		ShowPage();
	}
}
