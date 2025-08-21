using Godot;

public static class VectorExtensions
{
    public static Vector3 Lerp(this Vector3 from, Vector3 to, float t)
    {
        t = Mathf.Clamp(t, 0f, 1f);
        return new Vector3(
            Mathf.Lerp(from.X, to.X, t),
            Mathf.Lerp(from.Y, to.Y, t),
            Mathf.Lerp(from.Z, to.Z, t)
        );
    }

    public static Vector2 Lerp(this Vector2 from, Vector2 to, float t)
    {
        t = Mathf.Clamp(t, 0f, 1f);
        return new Vector2(
            Mathf.Lerp(from.X, to.X, t),
            Mathf.Lerp(from.Y, to.Y, t)
        );
    }
}
