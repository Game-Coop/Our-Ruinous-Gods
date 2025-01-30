
using System;
using Godot;

public class StartMenu : Control
{
	[Export] private NodePath startBtnPath;
	[Export] private NodePath exitBtnPath;
	[Export] private PackedScene gameScene;
	private Button _startBtn;
	private Button _exitBtn;
	public override void _Ready()
	{
		base._Ready();
		_startBtn = GetNode<Button>(startBtnPath);
		_exitBtn = GetNode<Button>(exitBtnPath);

		_startBtn.Connect("pressed", this, nameof(OnStartPressed));
		_exitBtn.Connect("pressed", this, nameof(OnExitPressed));
	}
	private void OnStartPressed()
	{
		_startBtn.Disabled = true;
		GetTree().ChangeSceneTo(gameScene);
	}
	private void OnExitPressed()
	{
		GetTree().Quit();
	}
}
