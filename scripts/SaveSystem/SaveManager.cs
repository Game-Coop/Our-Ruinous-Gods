
using System;
using System.Collections.Generic;
using Godot;

public partial class SaveManager : Node
{
	public static event Action OnBeforeLoad;
	public static event Action OnAfterLoad;
	public static event Action OnBeforeSave;
	public static event Action OnAfterSave;

	private static string defaultSaveName = "save";
	private static string savePath => OS.GetUserDataDir();
	private static ISaveSystem saveSystem;
	private static SaveData saveData;
	public static SaveData SaveData => saveData;
	public static bool HasSave => saveSystem.HasSave(defaultSaveName);
	public static bool HasSpecialSave
	{
		get
		{
			if (!HasSave) return false;

			if (saveData == null)
				saveSystem.Load(defaultSaveName, out saveData, false);

			return saveData.playerDiedBefore; // this could be a game state check as well
		}
	}
	public override void _Ready()
	{
		base._Ready();
		saveSystem = SaveFactory.CreateLocalSaveSystem(this, savePath, SaveFormat.Json);
	}

	public static void NewGame()
	{
		if (HasSave)
		{
			saveSystem.Clear(defaultSaveName);
		}
		saveData = new SaveData();
	}
	public static void ContinueGame()
	{
		if (!HasSave)
		{
			GD.PrintErr("There is no save to continue!");
			NewGame();
			return;
		}
		OnBeforeLoad?.Invoke();
		saveSystem.Load(defaultSaveName, out saveData);
		OnAfterLoad?.Invoke();
	}
	public static void Save()
	{
		OnBeforeSave?.Invoke();
		saveSystem.Save(defaultSaveName, saveData);
		OnAfterSave?.Invoke();
	}
	public static List<string> GetValidSaves()
	{
		return saveSystem.GetValidSaves(SaveFormat.Json);
	}
}
