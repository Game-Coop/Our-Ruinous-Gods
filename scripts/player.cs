using Godot;
using System;
using System.IO.Compression;

public class player : KinematicBody
{
    private Vector3 direction;
    [Export] private float gamepad_sensitivity = (float)1;
    private Spatial head;
    [Export] private float mouse_sensitivity = (float)1;
    [Export] private float speed = 1;
    [Export] private float inertia = 1;
    private Vector3 velocity;

    public override void _Ready()
    {
        EventBus EventBusHandler = GetNode<EventBus>("/root/EventBus");

        head = GetNode<Spatial>("Head");

        Input.MouseMode = Input.MouseModeEnum.Captured;
        EventBusHandler.Connect("PowerChangedEventHandler", this, "SomeFunction");
    }

    public void SomeFunction(PowerEvent e) {
        //GD.Print("test");
        GD.Print(e.Charge);
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

    private void HandelMove(float delta) {
        float horizontalRotion = GlobalTransform.basis.GetEuler().y;
        float movement = Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward");
        float strafe = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");

        direction = Vector3.Zero;
        direction = new Vector3(strafe, 0, movement);
        direction = direction.Rotated(Vector3.Up, horizontalRotion).Normalized();

        velocity = velocity.LinearInterpolate(direction * speed, delta / inertia);

        MoveAndSlide(velocity, Vector3.Up);
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
