using Godot;

public partial class FPSCamera : Camera3D
{
	[Export] private Camera3D mainCamera;

	public override void _Process(double delta)
	{
		base._Process(delta);
		GlobalTransform = mainCamera.GlobalTransform;
	}
}
