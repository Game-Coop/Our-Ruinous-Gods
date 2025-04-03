

using System;
using Godot;

public class Pagination : Control
{
	public event Action OnClick;
	[Export] private NodePath filledImagePath;
	[Export] private NodePath labelPath;
	private Control _filledImage;
	private Control filledImage
	{
		get
		{
			if (_filledImage == null)
			{
				_filledImage = GetNode<Control>(filledImagePath);
			}
			return _filledImage;
		}
	}
	private Label label;

	private void ButtonPressed()
	{
		OnClick?.Invoke();
	}
	public void SetTitle(string name)
	{
		label = GetNodeOrNull<Label>(labelPath);
		if (label != null)
		{
			label.Text = name;
		}
	}
	public void Select()
	{
		filledImage.Show();
	}
	public void UnSelect()
	{
		filledImage.Hide();
	}
}
