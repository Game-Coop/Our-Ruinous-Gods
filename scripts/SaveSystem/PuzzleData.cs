using Godot;

[GlobalClass]
public partial class PuzzleData : Resource
{
    [Export] public int Id { get; private set; }
    [Export] public string Name { get; private set; }
    [Export] public string Description { get; private set; } // description for how to solve the puzzle
    public bool IsSolved { get; set; }
}