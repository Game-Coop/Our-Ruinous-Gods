using Godot;

public static class UIExtensions
{
    public static float GetNormalizedValue(this Slider slider)
    {
        return Mathf.InverseLerp((float)slider.MinValue, (float)slider.MaxValue, (float)slider.Value);
    }
}