
using Godot;

public partial class AccessibiltySettingsMenu : Page
{
	[Export] private Slider fontScaleSlider;
	[Export] private Label fontScaleLabel;
	[Export] private float min;
	[Export] private float max;
	public override void _Ready()
	{
		base._Ready();
		fontScaleSlider.ValueChanged += FontScaleSliderChanged;
	}
	public override void ShowPage(bool instant = false)
	{
		base.ShowPage(instant);
		Initialize();
	}
	public override void HidePage(bool instant = false)
	{
		base.HidePage(instant);
	}
	public void Initialize()
	{
		var value = Mathf.InverseLerp(min, max, Settings.Accessibility.FontScale);
		fontScaleSlider.SetValueNoSignal(Mathf.Lerp(fontScaleSlider.MinValue, fontScaleSlider.MaxValue, value));
		fontScaleLabel.Text = $"x{Settings.Accessibility.FontScale:F1}";
	}
	private void FontScaleSliderChanged(double value)
	{
		var fontScale = Mathf.Lerp(min, max, fontScaleSlider.GetNormalizedValue());
		Settings.Accessibility.FontScale = (float)fontScale;
		fontScaleLabel.Text = $"x{Settings.Accessibility.FontScale:F1}";
	}
}
