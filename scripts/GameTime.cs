
using Godot;

public partial class GameTime : Node
{
    public static double Time => _time;
    private static double _time;
    public override void _Process(double delta)
    {
        base._Process(delta);
        _time += delta;
    }
}