using Godot;

public partial class Hint : Label3D
{
	[Export] private NodePath spritePath;

	private Sprite3D sprite;
	private Vector2 referenceScale = new Vector2(200f, 200f);
	public override void _Ready()
	{
		base._Ready();
		sprite = GetNode<Sprite3D>(spritePath);
		Visible = false;
	}
	public void Setup(string text, Texture2D texture)
	{
		float aspect = texture.GetHeight() / (float)texture.GetWidth();
		var scaleFactor = referenceScale / texture.GetSize();

		sprite.Scale = new Vector3(scaleFactor.x, scaleFactor.x * aspect, 1f);
		sprite.Texture2D = texture;
		Text = text;
	}
}
