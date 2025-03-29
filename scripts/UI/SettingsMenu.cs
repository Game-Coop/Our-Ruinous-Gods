
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
	private Button _continueBtn;
	private Button _exitBtn;
	private Button _controlsBtn;
	private Page controlsMenuPage;

	private Page currentPage;
	protected override void _Ready()
	{
		base._Ready();
		controlsMenuPage = GetNode<Page>(controlsMenuPath);

		_continueBtn = GetNode<Button>(continueBtnPath);
		_exitBtn = GetNode<Button>(exitBtnPath);
		_controlsBtn = GetNode<Button>(controlsBtnPath);

		_continueBtn.Connect("pressed", this, nameof(ContinuePressed));
		_exitBtn.Connect("pressed", this, nameof(OnExitPressed));
		_controlsBtn.Connect("pressed", this, nameof(ControlsPressed));

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
		currentPage = controlsMenuPage;

		if (!controlsMenuPage.Visible)
			controlsMenuPage.ShowPage();
		else
			controlsMenuPage.HidePage();
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
