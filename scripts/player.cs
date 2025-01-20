using Godot;
using System;
using System.IO.Compression;
using System.Security.Permissions;

public class player : KinematicBody
{
    [Export] private float gamepad_sensitivity = 1;
    [Export] private float gravity_radius = 10;  
    [Export] private float gravity = 9.8f;   
    [Export] private float inertia = 1; 
    [Export] private float mouse_sensitivity = 1;
    [Export] private float speed = 1;

    private Vector3 direction;
    private Spatial head;
    private Vector3 velocity;

    public override void _Ready()
    {
        head = GetNode<Spatial>("Head");

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent e)
    {
        if(e is InputEventMouseMotion) handleMouseLook(e as InputEventMouseMotion);
        if (e.IsActionPressed("quit")) GetTree().Quit();
    }

    public override void _PhysicsProcess(float delta) {
        float turnHorizontal = (Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left")) * gamepad_sensitivity;
        float turnVertical = (Input.GetActionStrength("look_down") - Input.GetActionStrength("look_up")) * gamepad_sensitivity;

        HandelMove(delta);
        HandelTurn(turnHorizontal, turnVertical);
    }

    private Vector3 GetUpAngle() {
        if(GlobalPosition.DistanceTo(Vector3.Zero) > gravity_radius) 
            return GlobalPosition.DirectionTo(Vector3.Zero);

        return Vector3.Up;
    }

    private Vector3 GetGravity(float delta) {
        /*
        Vector3 test = Vector3.Zero.DirectionTo(GlobalPosition);
        
        if(GlobalPosition.DistanceTo(Vector3.Zero) > gravity_radius) 
            return test * gravity * delta;
        */

        return Vector3.Down * gravity * delta;
    }
    private float TEST(Vector2 a, Vector2 b) {
    	Vector2 diff = new Vector2(a.x - b.x, a.y - b.y);
    
    	return Mathf.Atan2(diff.y, diff.x);
    }

    private void HandelMove(float delta) {
        float horizontalRotion = GlobalTransform.basis.GetEuler().y;
        float movement = Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward");
        float strafe = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");

        direction = Vector3.Zero;
        direction = new Vector3(strafe, 0, movement);
        direction = direction.Rotated(GetUpAngle(), horizontalRotion).Normalized();

        velocity = velocity.LinearInterpolate(direction * speed, delta / inertia);

        if(GlobalPosition.DistanceTo(Vector3.Zero) > gravity_radius) {
            GD.Print(Mathf.Rad2Deg(TEST(Vector2.Zero, ));
            Rotate(GlobalPosition.DirectionTo(Vector3.Zero), Mathf.Rad2Deg(TEST(Vector2.Zero, new Vector2(GlobalPosition.z, GlobalPosition.y))));
            RotateX(0.0f);
            /*Rotate(GlobalPosition, GlobalPosition.SignedAngleTo(Vector3.Zero, GetUpAngle()));*/    
        }

        MoveAndSlide(velocity + GetGravity(delta), GetUpAngle());
    }

    private void HandelTurn(float x, float y) {
        float clampDegrees = 90;

        RotateY(Mathf.Deg2Rad(-x));

        head.RotateX(Mathf.Deg2Rad(-y));
        head.Rotation = new Vector3(Mathf.Clamp(head.Rotation.x, Mathf.Deg2Rad(-clampDegrees), Mathf.Deg2Rad(clampDegrees)), head.Rotation.y, head.Rotation.z);
    }

    private void handleMouseLook(InputEventMouseMotion e) {
        HandelTurn(e.Relative.x * mouse_sensitivity, e.Relative.y * mouse_sensitivity);
    }
}
