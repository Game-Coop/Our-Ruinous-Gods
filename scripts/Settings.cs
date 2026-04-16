
using System;
using Godot;

public partial class Settings : Node, ISavable<SettingsData>
{
    public void OnLoad(SettingsData data)
    {
        Accessibility.FontScale = data.fontScale;
    }
    public void OnSave(SettingsData data)
    {
        data.fontScale = Accessibility.FontScale;
    }
    public static class Accessibility
    {
        public static event Action<float> OnFontScaleChange;
        private static float fontScale = 1f;
        public static float FontScale
        {
            get => fontScale;
            set
            {
                if (fontScale != value)
                {
                    fontScale = value;
                    OnFontScaleChange?.Invoke(fontScale);
                }
            }
        }
    }
    public static class Gameplay
    {

    }
    public static class Controls
    {
        
    }
    public static class Graphics
    {

    }
    public static class Audio
    {
        
    }
}