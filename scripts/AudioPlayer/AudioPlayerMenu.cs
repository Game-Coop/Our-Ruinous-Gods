using System;
using System.Collections.Generic;
using Godot;

public class AudioPlayerMenu : Page
{
	private SortedDictionary<int, AudioEntry> entries = new SortedDictionary<int, AudioEntry>();
	[Export] private PackedScene audioEntryTemplate;
	[Export] private NodePath entryContainerPath;
	[Export] private NodePath entryNameLabelPath;
	[Export] private NodePath sliderPath;
	[Export] private NodePath playButtonPath;
	[Export] private NodePath stopButtonPath;
	[Export] private NodePath rewindButtonPath;
	[Export] private NodePath fastForwardButtonPath;
	[Export] private NodePath playImagePath;
	[Export] private NodePath pauseImagePath;
	private Label entryNameLabel;
	private Node entryContainer;
	private HSlider slider;
	private Button playButton;
	private Button stopButton;
	private Button rewindButton;
	private Button fastForwardButton;
	private TextureRect playImage;
	private TextureRect pauseImage;
	private bool isDragging = false;

	protected override void _Ready()
	{
		base._Ready();

		entryContainer = GetNode(entryContainerPath);
		entryNameLabel = GetNode<Label>(entryNameLabelPath);
		slider = GetNode<HSlider>(sliderPath);

		playButton = GetNode<Button>(playButtonPath);
		stopButton = GetNode<Button>(stopButtonPath);
		rewindButton = GetNode<Button>(rewindButtonPath);
		fastForwardButton = GetNode<Button>(fastForwardButtonPath);

		playImage = GetNode<TextureRect>(playImagePath);
		pauseImage = GetNode<TextureRect>(pauseImagePath);

		playButton.Connect("pressed", this, nameof(PlayButtonPressed));
		stopButton.Connect("pressed", this, nameof(StopButtonPressed));
		rewindButton.Connect("pressed", this, nameof(RewindButtonPressed));
		fastForwardButton.Connect("pressed", this, nameof(FastForwardButtonPressed));

		AudioPlayerEvents.OnAudioCollect += OnEntryCollect;
		AudioPlayerEvents.OnAudioPlayerStoped += OnAudioPlayerStoped;
	}

	private void OnAudioPlayerStoped()
	{
		UpdatePlayImage();
	}

	private void StopButtonPressed()
	{
		AudioPlayer.Instance.End();
	}
	private void RewindButtonPressed()
	{
		AudioPlayer.Instance.Rewind(-5f);
	}
	private void FastForwardButtonPressed()
	{
		AudioPlayer.Instance.Rewind(5f);
	}
	private void PlayButtonPressed()
	{
		AudioPlayer.Instance.PausePlay(AudioPlayer.Instance.Playing);
		UpdatePlayImage();
	}

	private void UpdatePlayImage()
	{
		if (AudioPlayer.Instance.Playing)
		{
			playImage.Visible = false;
			pauseImage.Visible = true;
		}
		else
		{
			playImage.Visible = true;
			pauseImage.Visible = false;
		}
	}

	public override void ShowPage(bool instant = false)
	{
		base.ShowPage(instant);
		if (entryContainer.GetChildCount() > 0)
		{
			(entryContainer.GetChild(0) as AudioEntry).GrabFocus();
		}
	}
	private void OnEntryCollect(AudioData data)
	{
		AddEntry(data);
	}
	public void AddEntry(AudioData audioData)
	{
		if (entries.ContainsKey(audioData.Id))
		{
			GD.PrintErr("Item is already in inventory!");
			return;
		}
		var entry = audioEntryTemplate.Instance() as AudioEntry;
		entry.Setup(audioData);
		entry.OnFocus += OnAudioEntryFocus;
		entries.Add(audioData.Id, entry);

		entryContainer.AddChild(entry);
		ReOrderChilds();
		ConfigureFocus(entry);
	}
	private void ReOrderChilds()
	{
		int index = 0;
		foreach (var entry in entries)
		{
			entryContainer.MoveChild(entry.Value, index++);
		}
	}
	public void ConfigureFocus(AudioEntry entry)
	{
		var childCount = entryContainer.GetChildCount();
		int index = entry.GetIndex();
		var topEntry = index - 1 >= 0 ? entryContainer.GetChildOrNull<AudioEntry>(index - 1) : null;
		var bottomEntry = index + 1 < childCount ? entryContainer.GetChildOrNull<AudioEntry>(index + 1) : null;

		if (topEntry != null)
		{
			entry.FocusNeighbourTop = topEntry.GetPath();
			topEntry.FocusNeighbourBottom = entry.GetPath();
		}
		if (bottomEntry != null)
		{
			entry.FocusNeighbourBottom = bottomEntry.GetPath();
			bottomEntry.FocusNeighbourTop = entry.GetPath();
		}
	}
	private void OnAudioEntryFocus(AudioEntry entry)
	{
		entryNameLabel.Text = entry.audioData.Name;
		AudioPlayer.Instance.Setup(entry.audioData.AudioStreamSample);
	}
	public override void _Process(float delta)
	{
		base._Process(delta);
		if (isDragging || AudioPlayer.Instance.Stream == null) return;

		var normalizedPos = AudioPlayer.Instance.GetPlaybackPosition() / AudioPlayer.Instance.Stream.GetLength();
		var sliderProgress = Mathf.Lerp(0, 100, normalizedPos);
		slider.Value = sliderProgress;
	}
	public void OnSliderDragBegin()
	{
		isDragging = true;
	}
	public void OnSliderDragEnd(bool isChanged)
	{
		isDragging = false;
		var normalized = (float)slider.Value * 0.01f;

		AudioPlayer.Instance.SeekNormalized(normalized);
	}
}
