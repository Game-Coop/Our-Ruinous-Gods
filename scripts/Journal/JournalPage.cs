using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class JournalPage : Page
{
	private SortedDictionary<int, JournalEntry> entries = new SortedDictionary<int, JournalEntry>();
	[Export] private PackedScene journalEntryTemplate;
	[Export] private NodePath entryContainerPath;
	[Export] private NodePath entryNameLabelPath;
	[Export] private NodePath entryReaderPath;
	private JournalEntryReader entryReader;
	private Label entryNameLabel;
	private Node entryContainer;
	protected override void _Ready()
	{
		base._Ready();

		entryContainer = GetNode(entryContainerPath);
		entryNameLabel = GetNode<Label>(entryNameLabelPath);
		entryReader = GetNode<JournalEntryReader>(entryReaderPath);
		
		JournalEvents.OnUpdateRequest.Invoke();
	}
	public override void _EnterTree()
	{
		base._EnterTree();
		JournalEvents.OnJournalChange += OnJournalChange;
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		JournalEvents.OnJournalChange -= OnJournalChange;
	}
	public override void ShowPage(bool instant = false)
	{
		base.ShowPage(instant);
		if (entryContainer.GetChildCount() > 0)
		{
			(entryContainer.GetChild(0) as JournalEntry).GrabFocus();
		}
	}
	private void OnJournalChange(Dictionary<int, JournalData> journalDatas)
	{
		var entriesToRemove = entries.Where(pair => !journalDatas.ContainsKey(pair.Key));
		foreach (var entry in entriesToRemove)
		{
			RemoveEntry(entry.Value.journalData);
		}

		var entriesToAdd = journalDatas.Where(pair => !entries.ContainsKey(pair.Key));

		foreach (var item in entriesToAdd)
		{
			AddEntry(item.Value);
		}
	}
	private void RemoveEntry(JournalData journalData)
	{
		// TODO: Theoretically we won't be removing entries from journal but its here
		throw new NotImplementedException();
	}

	public void AddEntry(JournalData entryData)
	{
		if (entries.ContainsKey(entryData.Id))
		{
			GD.PrintErr("Item is already in inventory!");
			return;
		}
		var entry = journalEntryTemplate.Instance() as JournalEntry;
		entry.Setup(entryData);
		entry.OnFocus += OnEntryFocus;
		entries.Add(entryData.Id, entry);

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
	public void ConfigureFocus(JournalEntry entry)
	{
		var childCount = entryContainer.GetChildCount();
		int index = entry.GetIndex();
		var topEntry = index - 1 >= 0 ? entryContainer.GetChildOrNull<JournalEntry>(index - 1) : null;
		var bottomEntry = index + 1 < childCount ? entryContainer.GetChildOrNull<JournalEntry>(index + 1) : null;

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
	private void OnEntryFocus(JournalEntry entry)
	{
		entryNameLabel.Text = entry.journalData.Name;
		entryReader.Setup(entry.journalData);
	}

}
