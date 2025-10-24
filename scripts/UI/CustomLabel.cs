using Godot;

[GlobalClass]
public partial class CustomLabel : Label
{
	private int _baseFontSize;
	public override void _Ready()
	{
		_baseFontSize = GetThemeFontSize("font_size");
		UpdateFontSize(Settings.Accessibility.FontScale);
	}
	public override void _EnterTree()
	{
		base._EnterTree();
		Settings.Accessibility.OnFontScaleChange += OnFontScaleChanged;
		UpdateFontSize(Settings.Accessibility.FontScale);
	}
	public override void _ExitTree()
	{
		base._ExitTree();
		Settings.Accessibility.OnFontScaleChange -= OnFontScaleChanged;
	}
	private void OnFontScaleChanged(float scale)
	{
		UpdateFontSize(scale);
	}
	private void UpdateFontSize(float scale)
	{
		AddThemeFontSizeOverride("font_size", Mathf.RoundToInt(_baseFontSize * scale));
		// if (AutowrapMode != TextServer.AutowrapMode.Off)
		// {
		// 	var temp = Text;
		// 	Text = "";
		// 	Text = temp;
		// 	SetSize(Vector2.Zero); // Reset size
		// 	ResetSize(); // Force recalculation
		// }
	}
}
