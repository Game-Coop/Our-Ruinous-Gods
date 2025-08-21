using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class AudioPlayerMenu : Page
{
	private SortedDictionary<int, AudioEntry> entries = new SortedDictionary<int, AudioEntry>();
	[Export] private PackedScene audioEntryTemplate;
	[Export] private NodePath controlPanelPath;
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
	private AudioEntry focusedEntry;

	public override void _Ready()
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

		playButton.Connect("pressed", new Callable(this, nameof(PlayButtonPressed)));
		stopButton.Connect("pressed", new Callable(this, nameof(StopButtonPressed)));
		rewindButton.Connect("pressed", new Callable(this, nameof(RewindButtonPressed)));
		fastForwardButton.Connect("pressed", new Callable(this, nameof(FastForwardButtonPressed)));

		AudioPlayerEvents.OnUpdateRequest.Invoke();
	}
	public override void _EnterTree()
	{
		base._EnterTree();
		AudioPlayerEvents.OnAudioPlayerChange += OnAudioPlayerChange;
		AudioPlayerEvents.OnAudioPlayerStoped += OnAudioPlayerStoped;
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		AudioPlayerEvents.OnAudioPlayerChange -= OnAudioPlayerChange;
		AudioPlayerEvents.OnAudioPlayerStoped -= OnAudioPlayerStoped;
	}
	private void OnAudioPlayerChange(Dictionary<int, AudioData> audioDatas)
	{
		var entriesToRemove = entries.Where(pair => !audioDatas.ContainsKey(pair.Key)).Select(item => item.Value).ToList();
		foreach (var entry in entriesToRemove)
		{
			RemoveEntry(entry.audioData, false);
		}

		var entriesToAdd = audioDatas.Where(pair => !entries.ContainsKey(pair.Key));

		foreach (var item in entriesToAdd)
		{
			AddEntry(item.Value, false);
		}

		ReOrderChilds();
		ConfigureFocusAll();
	}
	private void OnEntryCollect(AudioData data)
	{
		AddEntry(data);
	}
	public void AddEntry(AudioData audioData, bool updateOrder = true)
	{
		if (entries.ContainsKey(audioData.Id))
		{
			GD.PrintErr("Item is already in inventory!");
			return;
		}
		var entry = audioEntryTemplate.Instantiate() as AudioEntry;
		entry.Setup(audioData);
		entry.OnFocus += OnAudioEntryFocus;
		entries.Add(audioData.Id, entry);

		entryContainer.AddChild(entry);
		if (updateOrder)
		{
			ReOrderChilds();
			ConfigureFocusAll();
		}
	}
	private void RemoveEntry(AudioData entryData, bool updateOrder = true)
	{
		if (entries.ContainsKey(entryData.Id))
		{
			var entry = entries[entryData.Id];
			entry.OnFocus -= OnAudioEntryFocus;
			entries.Remove(entryData.Id);
			entry.QueueFree();
			if (updateOrder)
			{
				ReOrderChilds();
				ConfigureFocusAll();
			}
		}
		else
		{
			GD.PrintErr("Item does not registered!");
		}
	}
	private void ReOrderChilds()
	{
		int index = 0;
		foreach (var entry in entries)
		{
			entryContainer.MoveChild(entry.Value, index++);
		}
	}
	private void ConfigureFocusAll()
	{
		foreach (var item in entries)
		{
			ConfigureFocus(item.Value);
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
			entry.FocusNeighborTop = topEntry.GetPath();
			topEntry.FocusNeighborBottom = entry.GetPath();
		}
		if (bottomEntry != null)
		{
			entry.FocusNeighborBottom = bottomEntry.GetPath();
			bottomEntry.FocusNeighborTop = entry.GetPath();
		}
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
		if (entryContainer.GetChildCount() > 0 && focusedEntry == null)
		{
			focusedEntry = entryContainer.GetChild(0) as AudioEntry;
			focusedEntry.GrabFocus();
			GetNode<Control>(controlPanelPath).Visible = true;
		}
	}

	private void OnAudioEntryFocus(AudioEntry entry)
	{
		entryNameLabel.Text = entry.audioData.Name;
		AudioPlayer.Instance.Setup(entry.audioData.AudioStreamWAV);
	}
	public override void _Process(double delta)
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
