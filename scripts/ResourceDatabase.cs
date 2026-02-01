using Godot;
using System.Collections.Generic;

public partial class ResourceDatabase : Node
{
	[Export] private Resources resources;
	public static PackedScene StartMenuScene { get; private set; }
	public static PackedScene GameScene { get; private set; }
	public static Dictionary<int, ItemData> ItemDatas { get; private set; } = new();
	public static Dictionary<int, JournalData> JournalDatas { get; private set; } = new();
	public static Dictionary<int, AudioData> AudioDatas { get; private set; } = new();
	public static Dictionary<int, PuzzleData> PuzzleDatas { get; private set; } = new();

	public override void _Ready()
	{
		base._Ready();

		StartMenuScene = resources.startMenuScene;
		GameScene = resources.gameScene;

		ItemDatas.Clear();
		JournalDatas.Clear();
		AudioDatas.Clear();
		PuzzleDatas.Clear();

		foreach (var item in resources.itemDatas)
		{
			ItemDatas.Add(item.Id, item);
		}
		foreach (var journalData in resources.journalDatas)
		{
			JournalDatas.Add(journalData.Id, journalData);
		}
		foreach (var audioData in resources.audioDatas)
		{
			AudioDatas.Add(audioData.Id, audioData);
		}
		foreach (var puzzleData in resources.puzzleDatas)
		{
			PuzzleDatas.Add(puzzleData.Id, puzzleData);
		}
	}

}