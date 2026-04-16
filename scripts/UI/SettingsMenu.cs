using Godot;

public partial class SettingsMenu : Page
{
	[Export] private Button _continueBtn;
	[Export] private Button _exitBtn;
	[Export] private Button _controlsBtn;
	[Export] private Button _gameplayBtn;
	[Export] private Button _graphicsBtn;
	[Export] private Button _audioBtn;
	[Export] private Button _accessibilityBtn;
	[Export] private Page controlsMenuPage;
	[Export] private Page gameplayMenuPage;
	[Export] private Page graphicsMenuPage;
	[Export] private Page audioMenuPage;
	[Export] private Page accessibilityPage;

	private Page currentPage;
	public override void _Ready()
	{
		base._Ready();
		_continueBtn.Pressed += ContinuePressed;
		_exitBtn.Pressed += OnExitPressed;
		_controlsBtn.Pressed += ControlsPressed;
		_gameplayBtn.Pressed += GameplayPressed;
		_graphicsBtn.Pressed += GraphicsPressed;
		_audioBtn.Pressed += AudioPressed;
		_accessibilityBtn.Pressed += AccessibilityPressed;

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
		GameManager.Instance.LoadStartMenu();
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
	private void AccessibilityPressed()
	{
		SelectPage(accessibilityPage);
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
