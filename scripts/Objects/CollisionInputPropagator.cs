
using Godot;

public partial class CollisionInputPropagator : Area3D
{
    [Export] private Camera3D camera;
    [Export] private MeshInstance3D sourceMesh;
    [Export] private SubViewport targetViewport;
    [Export(PropertyHint.Layers3DPhysics)]
    public uint collisionMask;
    // public override void _Input(InputEvent inputEvent)
    // {
    //     if (inputEvent is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
    //     {
    //         var mousePos = mouseEvent.Position;
    //         var origin = camera.ProjectRayOrigin(mousePos);
    //         var direction = camera.ProjectRayNormal(mousePos);
    //         var end = origin + direction * 1000;

    //         var spaceState = GetWorld3D().DirectSpaceState;
    //         var rayParams = new PhysicsRayQueryParameters3D
    //         {
    //             From = origin,
    //             To = end,
    //             CollisionMask = collisionMask,
    //             HitFromInside = true,
    //             CollideWithAreas = true
    //         };
    //         var result = spaceState.IntersectRay(rayParams);
    //         if (result != null && result.Count > 0)
    //         {
    //             var eventPos = result["position"].AsVector3();
    //             Vector3 local = sourceMesh.GlobalTransform.AffineInverse() * eventPos;

    //             float width = sourceMesh.Scale.X;
    //             float height = sourceMesh.Scale.Y;

    //             float x01 = ((local.X / width) + 1f) * 0.5f;
    //             float y01 = (1f - (local.Y / height)) * 0.5f;

    //             var x = Mathf.Lerp(0, targetViewport.Size.X, x01);
    //             var y = Mathf.Lerp(0, targetViewport.Size.Y, y01);
    //             var screenPos = new Vector2(x, y);

    //             var cloned = inputEvent.Duplicate() as InputEventMouseButton;
    //             cloned.Position = screenPos;

    //             targetViewport.PushInput(cloned);
    //         }
    //     }
    // }
    public void _on_input_event(Node _, InputEvent inputEvent, Vector3 eventPos, Vector3 normal, int index)
    {
        if (inputEvent is not InputEventMouseButton mouse)
        {
            targetViewport.PushInput(inputEvent);
            return;
        }
        Vector3 local = sourceMesh.GlobalTransform.AffineInverse() * eventPos;

        float width = sourceMesh.Scale.X;
        float height = sourceMesh.Scale.Y;

        float x01 = ((local.X / width) + 1f) * 0.5f;
        float y01 = (1f - (local.Y / height)) * 0.5f;

        var x = Mathf.Lerp(0, targetViewport.Size.X, x01);
        var y = Mathf.Lerp(0, targetViewport.Size.Y, y01);
        var screenPos = new Vector2(x, y);

        var cloned = mouse.Duplicate() as InputEventMouseButton;
        cloned.Position = screenPos;

        targetViewport.PushInput(cloned);
    }
}
