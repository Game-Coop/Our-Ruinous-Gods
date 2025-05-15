
using System;
using System.Collections.Generic;
using Godot;

public class AudioPlayer : AudioStreamPlayer
{
	public Dictionary<int, AudioData> audioDatas = new Dictionary<int, AudioData>();
	public static AudioPlayer Instance { get; private set; }
	private event Action OnFinished;
	public override void _Ready()
	{
		base._Ready();
		Instance = this;
	}
	public override void _EnterTree()
	{
		base._EnterTree();
		AudioPlayerEvents.OnAudioCollect += AddData;
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		AudioPlayerEvents.OnAudioCollect -= AddData;

	}
	private void AddData(AudioData data)
	{
		GD.Print("Audio entry added to AudioPlayer: " + data.Name);
		audioDatas.Add(data.Id, data);
		AudioPlayerChanged();
	}

	private void AudioPlayerChanged()
	{
		AudioPlayerEvents.OnAudioPlayerChange?.Invoke(audioDatas);
	}

	public override void _Input(InputEvent e)
	{
		base._Input(e);
		if (e.IsActionPressed("audioplayer_rewind"))
		{
			Rewind(5f);
		}
		else if (e.IsActionPressed("audioplayer_pause"))
		{
			PausePlay(Playing);
		}
	}
	public void Setup(AudioStreamSample sample)
	{
		if (Playing)
		{
			Stop();
		}
		Stream = sample;
		Seek(0);
	}
	public void Rewind(float seconds)
	{
		var target = Mathf.Max(0f, GetPlaybackPosition() + seconds);
		target = Mathf.Min(target, Stream.GetLength());
		Seek(target);
	}
	public void SeekNormalized(float normalized)
	{
		var pos = Stream.GetLength() * normalized;
		Seek(pos);
	}
	public void PlayNormalized(float normalized)
	{
		var pos = Stream.GetLength() * normalized;
		Seek(pos);
	}
	public void PausePlay(bool pause)
	{
		if (pause)
		{
			Stop();
		}
		else
		{
			Play(GetPlaybackPosition());
		}
	}
	public void End()
	{
		if (Playing)
		{
			OnFinished += () =>
			{
				Seek(0);
			};
		}
		else
		{
			Seek(0);
		}
		Stop();
	}
	private void OnStreamFinished()
	{
		AudioPlayerEvents.OnAudioPlayerStoped?.Invoke();
		OnFinished?.Invoke();
		OnFinished = null;

		var progress = GetPlaybackPosition() / Stream.GetLength();
		if (progress >= 1f)
		{
			Seek(0);
			AudioPlayerEvents.OnAudioPlayerFinished?.Invoke();
		}
	}
}
