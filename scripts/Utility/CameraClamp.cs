public struct CameraClamp
{
    public float Min { get; }
    public float Max { get; }

    public CameraClamp(float min, float max)
    {
        Min = min;
        Max = max;
    }

    public override string ToString() => $"({Min} - {Max})";
}