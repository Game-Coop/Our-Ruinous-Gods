
using System;
using Godot;

public partial class JournalEntry : Button
{
	public event Action<JournalEntry> OnFocus;
	[Export] private NodePath labelPath;
	[Export] public JournalData journalData;
	public void Setup(JournalData journalData)
	{
		this.journalData = journalData;
	}
	public override void _Ready()
	{
		base._Ready();
		GetNode<Label>(labelPath).Text = journalData.Name;
	}

	public void OnFocused()
	{
		OnFocus?.Invoke(this);
	}
}
