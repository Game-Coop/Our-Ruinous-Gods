using System;
using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public Player Player { get; private set; }
    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        GameEvents.OnRegisterPlayer += RegisterPlayer;
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        GameEvents.OnRegisterPlayer -= RegisterPlayer;
    }
    private void RegisterPlayer(Player player)
    {
        Player = player;
    }

    public void WarpPlayer(Vector3 position, Vector3? rotation = null)
    {
        Player.GlobalPosition = position;
        if (rotation != null)
            Player.GlobalRotation = (Vector3)rotation;
    }

    public bool InCutscene { get; set; }
    public bool InStartMenu { get; set; }

}