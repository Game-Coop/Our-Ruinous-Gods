using Godot;
using System;

public class CloneIDGen : CanvasLayer
{
	private Label _cloneIDLabel;
	
	public override void _Ready()
	{
		_cloneIDLabel = GetNode<Label>("CloneIDLabel");
		if (_cloneIDLabel == null)
		{
			GD.PrintErr("CloneIDLabel node not found.");
			return;
		}
		
		CharacterEvents.OnDie += OnPlayerDied;
	}
	
	public override void _ExitTree()
	{
		CharacterEvents.OnDie -= OnPlayerDied;
	}
	
	private void OnPlayerDied()
	{
		var newCloneId = GenerateRandomCloneId();
		_cloneIDLabel.Text = "$Clone ID: {newCloneId}";
		_cloneIDLabel.Show();
	}
	
	private string GenerateRandomCloneId()
	{
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		
		string id = "";
		for (int i =0; i < 6; i++)
		{
			id += rng.RandiRange(0, 9).ToString();
		}
		
		return id;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
