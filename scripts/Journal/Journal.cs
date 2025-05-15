using System.Collections.Generic;
using Godot;

public class Journal : Node
{
	public Dictionary<int, JournalData> journalDatas = new Dictionary<int, JournalData>();
	public override void _EnterTree()
	{
		base._EnterTree();
		JournalEvents.OnUpdateRequest += JournalChanged;
		JournalEvents.OnEntryCollect += AddData;
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		JournalEvents.OnUpdateRequest -= JournalChanged;
		JournalEvents.OnEntryCollect -= AddData;
	}
	private void AddData(JournalData data)
	{
		GD.Print("Journal entry added to journal: " + data.Name);
		journalDatas.Add(data.Id, data);
		JournalChanged();
	}
	private void RemoveItem(ItemData data)
	{
		if (journalDatas.ContainsKey(data.Id))
		{
			journalDatas.Remove(data.Id);
			JournalChanged();
		}
		else
		{
			GD.PrintErr("Tried to remove an item that doesn't exist");
		}
	}
	private void JournalChanged()
	{
		JournalEvents.OnJournalChange.Invoke(journalDatas);
	}
}
