using System;
using System.Threading.Tasks;
using Godot;

public partial class StartMenu : Control
{
	[Export] private SceneTransitioner sceneTransitioner;
	[Export] private Camera3D camera;
	[Export] private Button _newGameBtn;
	[Export] private Button _continueBtn;
	[Export] private Button _exitBtn;
	[Export] private Label _versionLabel;
	[Export] private AudioStreamPlayer _musicPlayer;
	private Node3D firstScene;
	public override void _Ready()
	{
		base._Ready();

		_musicPlayer.Play();
		Timer timer = new Timer();
		timer.WaitTime = _musicPlayer.Stream.GetLength();
		timer.Autostart = true;
		timer.OneShot = false;
		timer.Connect("timeout", new Callable(this, nameof(OnMusicTimerTimeout)));
		AddChild(timer);

		_versionLabel.Text = VersionInfo.Version;

		_newGameBtn.Connect("pressed", new Callable(this, nameof(NewGamePressed)));
		_continueBtn.Connect("pressed", new Callable(this, nameof(ContinueGamePressed)));
		_exitBtn.Connect("pressed", new Callable(this, nameof(OnExitPressed)));

		GameManager.Instance.InStartMenu = true;
		GameManager.Instance.InCutscene = false;

		firstScene = GetTree().LoadScene(ResourceDatabase.GameScene, true, true) as Node3D;
		camera.Current = true;
		if (!SaveManager.HasSave)
		{
			_newGameBtn.Visible = true;
			_continueBtn.Visible = false;
		}
		else
		{
			_newGameBtn.Visible = false;
			_continueBtn.Visible = true;
			if (SaveManager.HasSpecialSave)
			{
				//after death situation
			}
		}
	}
	private async void ContinueGamePressed()
	{
		_continueBtn.Disabled = true;
		SaveManager.ContinueGame();
		await StartLoadGame();
	}
	private async void NewGamePressed()
	{
		_newGameBtn.Disabled = true;
		SaveManager.NewGame();
		await StartLoadGame();
	}

	private async System.Threading.Tasks.Task StartLoadGame()
	{
		await FadeOutMusic();
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
		var fadeTween = CreateTween();
		fadeTween.TweenProperty(this, "modulate", new Color(0f, 0f, 0f, 0f), 0.3f);
		GameManager.Instance.InStartMenu = false;
		await LoadGame();
	}

	private async Task LoadGame()
	{
		if (!SaveManager.HasSave)
		{
			GameManager.Instance.InCutscene = true;
			GameManager.Instance.Player.SetPhysicsProcess(false);
			camera.Current = false;
			await sceneTransitioner.TransitionTo(firstScene, GameManager.Instance.Player, 20);
			GameManager.Instance.Player.SetPhysicsProcess(true);
			GameManager.Instance.InCutscene = false;
		}
		else
		{
			firstScene.QueueFree();
			GetTree().LoadScene(ResourceDatabase.GameScene);
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
