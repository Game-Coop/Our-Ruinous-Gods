
using System;
using System.Collections.Generic;
using System.IO;
using Godot;

public class SaveSystem : ISaveSystem
{
	private static IDecoder _decoder;
	private static ISaver _saver;
	private Node _nodeRef;
	public SaveSystem(Node nodeRef, ISaver saver, IDecoder decoder)
	{
		_nodeRef = nodeRef;
		_saver = saver;
		_decoder = decoder;
	}

	public void Save<T>(string saveFileName, T data)
	{
		ForEachSavable<T>((savable) =>
		{
			savable.OnSave(data);
		});
		_saver.Save(saveFileName, _decoder.Encode<T>(data));
	}

	public void Load<T>(string saveFileName, out T data, bool notify = true) where T : new()
	{
		data = _decoder.Decode<T>(_saver.Load(saveFileName));
		if (data != null)
		{
			T val = data;
			if (notify)
			{
				ForEachSavable<T>((savable) =>
				{
					savable.OnLoad(val);
				});
			}
		}
		else
		{
			data = new T();
		}
	}
	public List<string> GetValidSaves(SaveFormat format)
	{
		FileInfo[] files = _saver.GetAvailableSaves(format);
		List<string> validSaves = new List<string>();

		if (files != null)
		{
			foreach (var file in files)
			{
				validSaves.Add(file.Name);
			}
		}

		return validSaves;
	}
	public bool HasSave(string saveFileName)
	{
		return _saver.HasSave(saveFileName);
	}

	public void Clear(string saveFileName)
	{
		_saver.Delete(saveFileName);
	}
	public void ForEachSavable<T>(Action<ISavable<T>> action)
	{
		Traverse(_nodeRef.GetTree().Root, action);
	}

	private void Traverse<T>(Node node, Action<ISavable<T>> action)
	{
		if (node is ISavable<T> savable)
			action(savable);

		foreach (Node child in node.GetChildren())
			Traverse(child, action);
	}



}
