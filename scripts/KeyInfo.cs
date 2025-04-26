using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(KeyInfo), "", nameof(Resource))]
public class KeyInfo : Resource
{
	[Export] public string Name { get; set; }
	[Export] public string InputName { get; set; }
	[Export] public Texture Icon { get; set; }
	[Export] public InputLayout Layout { get; set; }
	public KeyInfo()
	{
		Name = "";
		InputName = "";
		Layout = InputLayout.KeyboardMouse;
		Icon = null;
	}
}
