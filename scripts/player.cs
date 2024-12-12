using Godot;
using System;
using System.IO.Compression;

public class player : KinematicBody
{
    private Vector3 direction;
    private Spatial head;
    private float mouse_sensitivity = (float)0.1;
    private float gamepad_sensitivity = (float)1.25;
    private float player_speed = 10000;

    public override void _Ready()
    {
        head = GetNode<Spatial>("Head");

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent e)
    {
        if(e is InputEventMouseMotion) handleMouseLook(e as InputEventMouseMotion);
    }

    public override void _PhysicsProcess(float delta) {
        float turnHorizontal = Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left") * gamepad_sensitivity;
        float turnVertical = Input.GetActionStrength("look_down") - Input.GetActionStrength("look_up") * gamepad_sensitivity;

        HandelMove();
        HandelTurn(turnHorizontal, turnVertical);
    }

    private void HandelMove() {
        float horizontalRotion = GlobalTransform.basis.GetEuler().y;
        float movement = (Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward")) * player_speed;
        float strafe = (Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left")) * player_speed;

        direction = Vector3.Zero;
        direction = new Vector3(strafe, 0, movement);
        direction = direction.Rotated(Vector3.Up, horizontalRotion).Normalized();

        MoveAndSlide(direction, Vector3.Up);
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
