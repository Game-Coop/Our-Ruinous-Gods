using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class InputBindings : Node
{
	[Export] private KeyInfo[] keyInfos;
	private static Dictionary<InputLayout, Dictionary<string, KeyInfo>> inputKeysDict = new Dictionary<InputLayout, Dictionary<string, KeyInfo>>();
	public static InputLayout CurrentLayout => InputLayoutTracker.CurrentLayout;
	public override void _Ready()
	{
		base._Ready();
		SetupBindings();
		InputLayoutTracker.OnLayoutChanged += OnLayoutChanged;
		// LoadInputBindings();
	}


	private void OnLayoutChanged(InputLayout layout)
	{
		GD.Print("Layout changed to: " + layout.ToString());
	}

	private void SetupBindings()
	{
		inputKeysDict.Clear();

		inputKeysDict.Add(InputLayout.KeyboardMouse, new Dictionary<string, KeyInfo>());
		inputKeysDict.Add(InputLayout.PlayStation, new Dictionary<string, KeyInfo>());
		inputKeysDict.Add(InputLayout.Xbox, new Dictionary<string, KeyInfo>());

		foreach (var keyInfo in keyInfos)
		{
			if (string.IsNullOrEmpty(keyInfo.InputName))
			{
				GD.PrintErr($"KeyInfo: {keyInfo.Name} does not have input name");
				continue;
			}
			if (keyInfo.Layout == InputLayout.KeyboardMouse)
			{
				inputKeysDict[InputLayout.KeyboardMouse].Add(keyInfo.InputName, keyInfo);
			}
			else if (keyInfo.Layout == InputLayout.PlayStation)
			{
				inputKeysDict[InputLayout.PlayStation].Add(keyInfo.InputName, keyInfo);
			}
			else if (keyInfo.Layout == InputLayout.Xbox)
			{
				inputKeysDict[InputLayout.Xbox].Add(keyInfo.InputName, keyInfo);
			}
		}
	}

	private void LoadInputBindings()
	{
		foreach (InputLayout layout in Enum.GetValues(typeof(InputLayout)))
		{
			var inputKeys = inputKeysDict[layout];
			foreach (var keyInfo in inputKeys.Values.ToList())
			{
				var inputName = keyInfo.InputName;
				inputKeysDict[layout][inputName] = GetSavedKeyInfo(inputName);
			}
		}
	}

	private KeyInfo GetSavedKeyInfo(string inputName)
	{
		// TODO: access saved key info from json
		throw new NotImplementedException();
	}

	private void SaveInputBindings()
	{
		throw new NotImplementedException();
	}

	public static KeyInfo GetKeyInfo(string inputName)
	{
		if (!inputKeysDict.ContainsKey(CurrentLayout))
		{
			GD.PrintErr("Missing layout entry");
			return null;
		}

		var keyInfo = inputKeysDict[CurrentLayout][inputName];
		return keyInfo;
	}
	public static void SetBinding(InputLayout layout, string inputName, KeyInfo keyInfo)
	{
		inputKeysDict[layout][inputName] = keyInfo;
		if (keyInfo != null)
			keyInfo.InputName = inputName;
	}
	public static void SwapKeyboardKeys(string inputName1, string inputName2)
	{
		var keyInfo1 = string.IsNullOrEmpty(inputName1) ? null : inputKeysDict[InputLayout.KeyboardMouse][inputName1];
		var keyInfo2 = string.IsNullOrEmpty(inputName2) ? null : inputKeysDict[InputLayout.KeyboardMouse][inputName2];

		SetBinding(InputLayout.KeyboardMouse, inputName1, keyInfo2);
		SetBinding(InputLayout.KeyboardMouse, inputName2, keyInfo1);

		InputRebinder.SwapKeyboardBindings(inputName1, inputName2);
	}
	public static void SwapGamepadKeys(string inputName1, string inputName2)
	{
		var keyInfoPS1 = string.IsNullOrEmpty(inputName1) ? null : inputKeysDict[InputLayout.PlayStation][inputName1];
		var keyInfoPS2 = string.IsNullOrEmpty(inputName2) ? null : inputKeysDict[InputLayout.PlayStation][inputName2];

		var keyInfoXbox1 = string.IsNullOrEmpty(inputName1) ? null : inputKeysDict[InputLayout.Xbox][inputName1];
		var keyInfoXbox2 = string.IsNullOrEmpty(inputName2) ? null : inputKeysDict[InputLayout.Xbox][inputName2];

		SetBinding(InputLayout.PlayStation, inputName1, keyInfoPS2);
		SetBinding(InputLayout.PlayStation, inputName2, keyInfoPS1);

		SetBinding(InputLayout.Xbox, inputName1, keyInfoXbox2);
		SetBinding(InputLayout.Xbox, inputName2, keyInfoXbox1);

		InputRebinder.SwapGamepadBindings(inputName1, inputName2);
	}
}
