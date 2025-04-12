
using System;
using System.Collections.Generic;
using Godot;

public class AudioSettingsMenu : Page
{
	[Export] private NodePath masterSliderPath;
	[Export] private NodePath dialogSliderPath;
	[Export] private NodePath sfxSliderPath;
	[Export] private NodePath musicSliderPath;
	[Export] private NodePath envrionmentSliderPath;

	private Slider _masterSlider;
	private Slider _dialogSlider;
	private Slider _sfxSlider;
	private Slider _musicSlider;
	private Slider _environmentSlider;

	protected override void _Ready()
	{
		base._Ready();

		_masterSlider = GetNode<Slider>(masterSliderPath);
		_dialogSlider = GetNode<Slider>(dialogSliderPath);
		_sfxSlider = GetNode<Slider>(sfxSliderPath);
		_musicSlider = GetNode<Slider>(musicSliderPath);
		_environmentSlider = GetNode<Slider>(envrionmentSliderPath);

		_masterSlider.Connect("value_changed", this, nameof(MasterSliderChanged));
		_dialogSlider.Connect("value_changed", this, nameof(DialogSliderChanged));
		_sfxSlider.Connect("value_changed", this, nameof(SfxSliderSliderChanged));
		_musicSlider.Connect("value_changed", this, nameof(MusicSliderChanged));
		_environmentSlider.Connect("value_changed", this, nameof(EnvironmentSliderChanged));
	}
	public override void ShowPage(bool instant = false)
	{
		base.ShowPage(instant);
		LoadAudioSettings();
	}
	public override void HidePage(bool instant = false)
	{
		base.HidePage(instant);
	}
	private void LoadAudioSettings()
	{
		_masterSlider.Value = Mathf.Lerp((float)_masterSlider.MinValue, (float)_masterSlider.MaxValue, AudioMixer.MasterVolume);
		_dialogSlider.Value = Mathf.Lerp((float)_dialogSlider.MinValue, (float)_dialogSlider.MaxValue, AudioMixer.DialogVolume);
		_musicSlider.Value = Mathf.Lerp((float)_musicSlider.MinValue, (float)_musicSlider.MaxValue, AudioMixer.MusicVolume);
		_sfxSlider.Value = Mathf.Lerp((float)_sfxSlider.MinValue, (float)_sfxSlider.MaxValue, AudioMixer.SfxVolume);
		_environmentSlider.Value = Mathf.Lerp((float)_environmentSlider.MinValue, (float)_environmentSlider.MaxValue, AudioMixer.EnvironmentVolume);
	}
	private void MasterSliderChanged(float val)
	{
		float normalizedVal = _masterSlider.GetNormalizedValue();
		AudioMixer.MasterVolume = normalizedVal;
	}
	private void DialogSliderChanged(float val)
	{
		float normalizedVal = _dialogSlider.GetNormalizedValue();
		AudioMixer.DialogVolume = normalizedVal;
	}

	private void SfxSliderSliderChanged(float val)
	{
		float normalizedVal = _sfxSlider.GetNormalizedValue();
		AudioMixer.SfxVolume = normalizedVal;
	}

	private void MusicSliderChanged(float val)
	{
		float normalizedVal = _musicSlider.GetNormalizedValue();
		AudioMixer.MusicVolume = normalizedVal;
	}

	private void EnvironmentSliderChanged(float val)
	{
		float normalizedVal = _environmentSlider.GetNormalizedValue();
		AudioMixer.EnvironmentVolume = normalizedVal;
	}

}
