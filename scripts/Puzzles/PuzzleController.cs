using System;
using System.Collections.Generic;
using Godot;

public partial class PuzzleController : Node, ISavable<SaveData>
{
	EventBus eventBus;
	private IPuzzle currentPuzzle;
	private List<PuzzleData> solvedPuzzles = new List<PuzzleData>();
	public override void _EnterTree()
	{
		base._EnterTree();
		eventBus = GetNode<EventBus>("/root/EventBus");
		eventBus.PuzzleInteract += OnPuzzleInteract;
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		eventBus.PuzzleInteract -= OnPuzzleInteract;
	}
	public override void _Input(InputEvent @event)
	{
		if (currentPuzzle == null) return;

		if (@event.IsAction("move_forward") || @event.IsAction("move_backward") || @event.IsAction("move_left") || @event.IsAction("move_right"))
		{
			currentPuzzle.Input(Input.GetVector("move_left", "move_right", "move_backward", "move_forward"));
		}
		else if (@event.IsAction("submit"))
		{
			currentPuzzle.Submit();
		}
		else if (@event.IsActionPressed("back"))
		{
			currentPuzzle.Back();
		}
		else if (@event.IsActionPressed("reset"))
		{
			currentPuzzle.Reset();
		}
	}

	private void OnPuzzleInteract(PuzzleInteractEvent puzzleInteractEvent)
	{
		currentPuzzle = puzzleInteractEvent.puzzle;

		currentPuzzle.OnBack += CurrentPuzzleBack;
		currentPuzzle.OnSolve += CurrentPuzzleSolve;
	}

	private void CurrentPuzzleSolve(PuzzleData puzzleData)
	{
		currentPuzzle.OnSolve -= CurrentPuzzleSolve;

		solvedPuzzles.Add(puzzleData);
	}

	private void CurrentPuzzleBack()
	{
		currentPuzzle.OnBack -= CurrentPuzzleBack;
		currentPuzzle.OnSolve -= CurrentPuzzleSolve;
		currentPuzzle = null;
	}

	public void OnSave(SaveData data)
	{
		data.solvedPuzzleIds.Clear();
		foreach (var solvedPuzzle in solvedPuzzles)
		{
			data.solvedPuzzleIds.Add(solvedPuzzle.Id);
		}
	}

	public void OnLoad(SaveData data)
	{
		foreach (var id in data.solvedPuzzleIds)
		{
			var puzzleData = ResourceDatabase.PuzzleDatas[id];
			puzzleData.IsSolved = true;
			solvedPuzzles.Add(puzzleData);
		}
	}
}
