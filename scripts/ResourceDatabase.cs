using Godot;
using System;
using System.Collections.Generic;

public partial class ResourceDatabase : Node
{
	[Export] private PackedScene startMenuScene;
	[Export] private PackedScene gameScene;
	[Export] private ItemData[] itemDatas;
	[Export] private JournalData[] journalDatas;
	[Export] private AudioData[] audioDatas;

	public static PackedScene StartMenuScene { get; private set; }
	public static PackedScene GameScene { get; private set; }
	public static Dictionary<int, ItemData> ItemDatas { get; private set; } = new Dictionary<int, ItemData>();
	public static Dictionary<int, JournalData> JournalDatas { get; private set; } = new Dictionary<int, JournalData>();
	public static Dictionary<int, AudioData> AudioDatas { get; private set; } = new Dictionary<int, AudioData>();

	public override void _Ready()
	{
		base._Ready();
		
		StartMenuScene = startMenuScene;
		GameScene = gameScene;

		ItemDatas.Clear();
		JournalDatas.Clear();
		AudioDatas.Clear();
		foreach (var item in itemDatas)
		{
			ItemDatas.Add(item.Id, item);
		}
		foreach (var journalData in journalDatas)
		{
			JournalDatas.Add(journalData.Id, journalData);
		}
		foreach (var audioData in audioDatas)
		{
			AudioDatas.Add(audioData.Id, audioData);
		}
	}

}
