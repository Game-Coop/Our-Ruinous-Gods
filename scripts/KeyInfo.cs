using Godot;

[RegisteredType(nameof(KeyInfo), "", nameof(Resource))]
public partial class KeyInfo : Resource
{
	[Export] public string Name { get; set; }
	[Export] public string InputName { get; set; }
	[Export] public Texture2D Icon { get; set; }
	[Export] public InputLayout Layout { get; set; }
	public KeyInfo()
	{
		Name = "";
		InputName = "";
		Layout = InputLayout.KeyboardMouse;
		Icon = null;
	}
}
