using Godot;

public partial class KeybindLabel3D : Label3D
{
    [Export] private string inputName;
    [Export] private Sprite3D sprite3d;
    public override void _Ready()
    {
        base._Ready();
        sprite3d.Texture = InputBindings.GetKeyInfo(inputName).Icon;
    }
}