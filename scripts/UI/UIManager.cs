using System;
using Godot;
public class UIManager : CanvasLayer
{
	[Export] private NodePath pauseMenuPagePath;
	[Export] private NodePath settingsMenuPagePath;
	private PauseMenu pauseMenuPage;
	private SettingsMenu settingsMenuPage;

	public override void _Ready()
	{
		base._Ready();

		pauseMenuPage = GetNode<PauseMenu>(pauseMenuPagePath);
		settingsMenuPage = GetNode<SettingsMenu>(settingsMenuPagePath);
	}
	public bool AnyPageVisible => pauseMenuPage.Visible || settingsMenuPage.Visible;
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (!AnyPageVisible)
		{
			// TODO: if we want to lock opening these pages with progression we can do it here
			if (@event.IsActionPressed("inventory_toggle"))
			{
				GetTree().SetInputAsHandled();
				pauseMenuPage.OpenInventory();
			}
			else if (@event.IsActionPressed("journal_toggle"))
			{
				GetTree().SetInputAsHandled();
				pauseMenuPage.OpenJournal();
			}
			else if (@event.IsActionPressed("audioplayer_toggle"))
			{
				GetTree().SetInputAsHandled();
				pauseMenuPage.OpenAudioPlayer();
			}
			else if (@event.IsActionPressed("settings_toggle"))
			{
				GetTree().SetInputAsHandled();
				settingsMenuPage.OpenSettings();
			}
		}
	}
}
