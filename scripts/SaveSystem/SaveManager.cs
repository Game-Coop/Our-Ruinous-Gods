
using System;
using System.Collections.Generic;
using Godot;

public class SaveManager : Node
{
	public static event Action OnBeforeSaveLoad;
	public static event Action OnAfterSaveLoad;
	public static event Action OnBeforeSave;
	public static event Action OnAfterSave;

	private static string defaultSaveName = "save";
	private static string savePath => OS.GetUserDataDir();
	private static ISaveSystem saveSystem;
	private static SaveData saveData;
	public static bool HasSave => saveSystem.HasSave(defaultSaveName);
	
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
		OnBeforeSaveLoad?.Invoke();
		saveSystem.Load(defaultSaveName, out saveData);
		OnAfterSaveLoad?.Invoke();
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
