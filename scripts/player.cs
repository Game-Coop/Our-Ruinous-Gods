using Godot;
using System;
using System.IO.Compression;
using System.Security.Permissions;
using System.Xml.Schema;

public class player : KinematicBody
{
    [Export] private float gamepad_sensitivity = 1;
    [Export] private float gravity_radius = 10;  
    [Export] private float gravity = 9.8f;   
    [Export] private float mouse_sensitivity = 1;
    [Export] private float speed = 1;

    private Spatial head;
    private Vector3 velocity;

    public override void _Ready()
    {
        head = GetNode<Spatial>("Head");

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    public override void _Input(InputEvent e)
    {
        if (e.IsActionPressed("quit")) GetTree().Quit();
        if (e is InputEventMouseMotion) handleMouseLook(e as InputEventMouseMotion);
    }
    public override void _PhysicsProcess(float delta) {
        float turnHorizontal = (Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left")) * gamepad_sensitivity;
        float turnVertical = (Input.GetActionStrength("look_down") - Input.GetActionStrength("look_up")) * gamepad_sensitivity;

        HandelMove(delta);
        HandelLook(turnHorizontal, turnVertical);
    }
     private Vector3 GetUpDirection() {
        Vector3 angleVector = new Vector3(0, 0, GlobalPosition.z);

        if(GlobalPosition.DistanceTo(angleVector) > gravity_radius) 
            return GlobalPosition.DirectionTo(angleVector).Normalized();

        return Vector3.Up;
    }
    private Vector3 GetDownDirection() {
        Vector3 angleVector = new Vector3(0, 0, GlobalPosition.z);

        if(GlobalPosition.DistanceTo(angleVector) > gravity_radius) 
            return angleVector.DirectionTo(GlobalPosition).Normalized();

        return Vector3.Down;
    }
    private void HandelMove(float delta) {
        float velocity_y;

        if(GlobalPosition.DistanceTo(new Vector3(0, 0, GlobalPosition.z)) > gravity_radius) {
            Transform up_transform = AlignPlayerUp(GlobalTransform, GlobalPosition.DirectionTo(new Vector3(0, 0, GlobalPosition.z)));
        
            GlobalTransform = up_transform;
        }

        velocity += Vector3.Down * gravity * delta;
        
        velocity_y = velocity.y;

        velocity = Vector3.Zero;
        velocity += GlobalTransform.basis.z * ((Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward")) * (speed * 250) * delta);
        velocity += GlobalTransform.basis.x * ((Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left")) * (speed * 250) * delta);
        velocity.y = velocity_y;

        velocity = MoveAndSlideWithSnap(velocity, Vector3.Down  * 2, Vector3.Up);
    }
    private Transform AlignPlayerUp(Transform transform, Vector3 y) {
        transform.basis.y = y;
        transform.basis.x = -transform.basis.z.Cross(y);
        transform.basis = transform.basis.Orthonormalized();

        return transform;
    }
    private void HandelLook(float x, float y) {
        float clampDegrees = 90;

        RotateObjectLocal(Vector3.Up, Mathf.Deg2Rad(-x));

        head.RotateX(Mathf.Deg2Rad(-y));
        head.Rotation = new Vector3(Mathf.Clamp(head.Rotation.x, Mathf.Deg2Rad(-clampDegrees), Mathf.Deg2Rad(clampDegrees)), head.Rotation.y, head.Rotation.z);
    }
    private void handleMouseLook(InputEventMouseMotion e) {
        HandelLook(e.Relative.x * mouse_sensitivity, e.Relative.y * mouse_sensitivity);
    }
}