using Godot;

public partial class SkyboxController : Node
{
    [Export] public Vector3 SkyboxVelocity { get; set; }
    [Export] public bool RotationEnabled { get; set; }
    [Export] private Environment environment;
    public override void _Process(double delta)
    {
        base._Process(delta);
        if(RotationEnabled)
            environment.SkyRotation += SkyboxVelocity * (float)delta;
    }
}