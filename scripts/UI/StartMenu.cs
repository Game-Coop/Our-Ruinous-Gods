using System;
using System.Threading.Tasks;
using Godot;

public partial class StartMenu : Control
{
	[Export] private NodePath startBtnPath;
	[Export] private NodePath exitBtnPath;
	[Export] private NodePath musicPlayerPath;
	[Export] private NodePath versionLabelPath;
	[Export] private SceneTransitioner sceneTransitioner;
	[Export] private Camera3D camera;
	private Button _startBtn;
	private Button _exitBtn;
	private AudioStreamPlayer _musicPlayer;
	private Label _versionLabel;
	private Node3D firstScene;
	private bool IsFirstTime { get; set; } = true;
	public override void _Ready()
	{
		base._Ready();
		_startBtn = GetNode<Button>(startBtnPath);
		_exitBtn = GetNode<Button>(exitBtnPath);
		_versionLabel = GetNode<Label>(versionLabelPath);

		_musicPlayer = GetNode<AudioStreamPlayer>(musicPlayerPath);
		_musicPlayer.Play();
		Timer timer = new Timer();
		timer.WaitTime = _musicPlayer.Stream.GetLength();
		timer.Autostart = true;
		timer.OneShot = false;
		timer.Connect("timeout", new Callable(this, nameof(OnMusicTimerTimeout)));
		AddChild(timer);

		_versionLabel.Text = VersionInfo.Version;

		_startBtn.Connect("pressed", new Callable(this, nameof(OnStartPressed)));
		_exitBtn.Connect("pressed", new Callable(this, nameof(OnExitPressed)));
		if (IsFirstTime)
		{
			firstScene = ResourceDatabase.GameScene.Instantiate() as Node3D;
			GetTree().Root.CallDeferred("add_child", firstScene);
		}
	}

	private async void OnStartPressed()
	{
		_startBtn.Disabled = true;
		await FadeOutMusic();
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
		LoadScene();
	}

	private void LoadScene()
	{
		if (IsFirstTime)
		{
			var fadeTween = CreateTween();
			fadeTween.TweenProperty(this, "modulate", new Color(0f, 0f, 0f, 0f), 0.3f);
			sceneTransitioner.TransitionTo(firstScene, camera, 20);
		}
		else
		{
			GetTree().ChangeSceneToPacked(ResourceDatabase.GameScene);
		}
	}

	private void OnExitPressed()
	{
		GetTree().Quit();
	}

	private async System.Threading.Tasks.Task FadeOutMusic()
	{
		float fadeTime = 1.5f;
		float startVolume = _musicPlayer.VolumeDb;
		float endVolume = -80f;

		float elapsedTime = 0f;

		while (elapsedTime < fadeTime)
		{
			float t = elapsedTime / fadeTime;
			_musicPlayer.VolumeDb = Mathf.Lerp(startVolume, endVolume, t);
			await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
			elapsedTime += 0.05f;
		}

		_musicPlayer.VolumeDb = endVolume;
		_musicPlayer.Stop();
	}

	private void OnMusicTimerTimeout()
	{
		_musicPlayer.Play();
	}
}
