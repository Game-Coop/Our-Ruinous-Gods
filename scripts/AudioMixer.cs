using System;
using Godot;

public class AudioMixer : Node
{
    [Export] private AudioBusLayout audioBusLayout;
    private static int masterVolumeIndex = 0;
    private static int musicVolumeIndex = 1;
    private static int sfxVolumeIndex = 2;
    private static int environmentVolumeIndex = 3;
    private static int dialogVolumeIndex = 4;
    public static float MasterVolume
    {
        get
        {
            float val = Mathf.InverseLerp(-80f, 0f, AudioServer.GetBusVolumeDb(masterVolumeIndex));
            return val;
        }
        set
        {
            float db = Mathf.Lerp(-80f, 0f, value);
            AudioServer.SetBusVolumeDb(masterVolumeIndex, db);
        }
    }
    public static float MusicVolume
    {
        get
        {
            float val = Mathf.InverseLerp(-80f, 0f, AudioServer.GetBusVolumeDb(musicVolumeIndex));
            return val;
        }
        set
        {
            float db = Mathf.Lerp(-80f, 0f, value);
            AudioServer.SetBusVolumeDb(musicVolumeIndex, db);
        }
    }
    public static float SfxVolume
    {
        get
        {
            float val = Mathf.InverseLerp(-80f, 0f, AudioServer.GetBusVolumeDb(sfxVolumeIndex));
            return val;
        }
        set
        {
            float db = Mathf.Lerp(-80f, 0f, value);
            AudioServer.SetBusVolumeDb(sfxVolumeIndex, db);
        }
    }
    public static float EnvironmentVolume
    {
        get
        {
            float val = Mathf.InverseLerp(-80f, 0f, AudioServer.GetBusVolumeDb(environmentVolumeIndex));
            return val;
        }
        set
        {
            float db = Mathf.Lerp(-80f, 0f, value);
            AudioServer.SetBusVolumeDb(environmentVolumeIndex, db);
        }
    }
    public static float DialogVolume
    {
        get
        {
            float val = Mathf.InverseLerp(-80f, 0f, AudioServer.GetBusVolumeDb(dialogVolumeIndex));
            return val;
        }
        set
        {
            float db = Mathf.Lerp(-80f, 0f, value);
            AudioServer.SetBusVolumeDb(dialogVolumeIndex, db);
        }
    }
    public override void _Ready()
    {
        base._Ready();
        AudioServer.SetBusLayout(audioBusLayout);
        LoadSettings();
    }

    private void LoadSettings()
    {
        // TODO: load it from save
    }
}