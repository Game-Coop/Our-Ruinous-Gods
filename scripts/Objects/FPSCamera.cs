using Godot;

public partial class FPSCamera : Camera3D
{
	[Export] private Node3D mainCamera;

	public override void _Process(double delta)
	{
		base._Process(delta);
		GlobalTransform = mainCamera.GlobalTransform;
	}
}
