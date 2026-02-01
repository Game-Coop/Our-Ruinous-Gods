using Godot;

[GlobalClass]
public partial class Resources : Resource
{
	[Export] public PackedScene startMenuScene;
	[Export] public PackedScene gameScene;
	[Export] public ItemData[] itemDatas;
	[Export] public JournalData[] journalDatas;
	[Export] public AudioData[] audioDatas;
	[Export] public PuzzleData[] puzzleDatas;
}
