using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }
    public bool InCutscene { get; set; }
    public bool InStartMenu { get; set; }

}