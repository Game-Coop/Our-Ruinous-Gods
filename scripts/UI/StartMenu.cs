using System;
using System.Threading.Tasks;
using Godot;

public class StartMenu : Control
{
	[Export] private NodePath startBtnPath;
	[Export] private NodePath exitBtnPath;
	[Export] private NodePath musicPlayerPath;

	private Button _startBtn;
	private Button _exitBtn;
	private AudioStreamPlayer _musicPlayer;

	public override void _Ready()
	{
		base._Ready();
		_startBtn = GetNode<Button>(startBtnPath);
		_exitBtn = GetNode<Button>(exitBtnPath);
        _musicPlayer = GetNode<AudioStreamPlayer>(musicPlayerPath);
		_musicPlayer.Play();

		Timer timer = new Timer();
		timer.WaitTime = _musicPlayer.Stream.GetLength();
		timer.Autostart = true;
        timer.OneShot = false;
        timer.Connect("timeout", this, nameof(OnMusicTimerTimeout));
        AddChild(timer);

        _startBtn.Connect("pressed", this, nameof(OnStartPressed));
		_exitBtn.Connect("pressed", this, nameof(OnExitPressed));
	}

	private async void OnStartPressed()
	{
		_startBtn.Disabled = true;
        await FadeOutMusic();
        GetTree().Paused = false;
		GetTree().ChangeSceneTo(ResourceDatabase.GameScene);
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
