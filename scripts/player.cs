using static Godot.GD;

using Godot;
using System;

public class player : KinematicBody
{
    //private Vector3 direction;

    private Spatial head;
    private float sensitivity = (float)0.1;

    public override void _Ready()
    {
        head = GetNode<Spatial>("Head");

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    public override void _Input(InputEvent e)
    {
        float clampDegrees = 90;

        InputEventMouseMotion mouseMotionEvent = e as InputEventMouseMotion;
        if (mouseMotionEvent != null)
        {
            RotateY(Mathf.Deg2Rad(-mouseMotionEvent.Relative.x * sensitivity));
            
            head.RotateX(Mathf.Deg2Rad(-mouseMotionEvent.Relative.y * sensitivity));
            head.Rotation = new Vector3(Mathf.Clamp(head.Rotation.x, Mathf.Deg2Rad(-clampDegrees), Mathf.Deg2Rad(clampDegrees)), head.Rotation.y, head.Rotation.z);
        }
    }
}
