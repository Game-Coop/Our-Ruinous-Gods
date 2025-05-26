using Godot;

public partial class AudioMixer : Node
{
	[Export] private AudioBusLayout audioBusLayout;

	public static float GetVolume(AudioMixerGroup audioMixerGroup)
	{
		float val = Mathf.InverseLerp(-80f, 0f, AudioServer.GetBusVolumeDb((int)audioMixerGroup));
		return val;
	}
	public static void SetVolume(AudioMixerGroup audioMixerGroup, float value)
	{
		float db = Mathf.Lerp(-80f, 0f, value);
		AudioServer.SetBusVolumeDb((int)audioMixerGroup, db);
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
