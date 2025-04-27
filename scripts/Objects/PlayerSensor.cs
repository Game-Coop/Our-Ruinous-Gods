
using Godot;

public class PlayerSensor : Area
{
    [Export] private NodePath switchablePath;
    private ISwitchable switchable;
    public override void _Ready()
    {
        switchable = GetNode<ISwitchable>(switchablePath);
        Monitoring = true;
        Connect("body_entered", this, "OnBodyEntered");
        Connect("body_exited", this, "OnBodyExited");
    }
    public void OnBodyEntered(Node body)
    {
        if (body is player)
        {
            switchable.TurnOn();
        }
    }
    public void OnBodyExited(Node body)
    {
        if (body is player)
        {
            switchable.TurnOff();
        }
    }
}