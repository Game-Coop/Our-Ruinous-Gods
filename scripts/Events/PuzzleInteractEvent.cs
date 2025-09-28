using Godot;

public partial class PuzzleInteractEvent : GodotObject
{
    public BasePuzzle puzzle;

    public PuzzleInteractEvent(BasePuzzle puzzle)
    {
        this.puzzle = puzzle;
    }
}