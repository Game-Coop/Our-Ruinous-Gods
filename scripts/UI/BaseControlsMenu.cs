using System;
using System.Collections.Generic;
using Godot;
public abstract partial class BaseControlsMenu : Page
{
	[Export] private NodePath[] inputBindingPaths;
	[Export] private NodePath swapLabelPath;
	private List<InputBindingButton> inputBindings = new List<InputBindingButton>();
	private InputBindingButton selectedBinding;
	private Label swapLabel;
	public virtual void SwapKeys(InputBindingButton inputBinding1, InputBindingButton inputBinding2)
	{
		var text1 = inputBinding1.Text;
		inputBinding1.Text = inputBinding2.Text;
		inputBinding2.Text = text1;
	}
	protected override void _Ready()
	{
		base._Ready();
		swapLabel = GetNode<Label>(swapLabelPath);

		foreach (var binding in inputBindingPaths)
		{
			var inputBinding = GetNode<InputBindingButton>(binding);
			inputBindings.Add(inputBinding);
			inputBinding.OnBindingSelected += OnBindingSelect;
		}
	}

	private void OnBindingSelect(InputBindingButton binding)
	{
		if (selectedBinding == null)
		{
			selectedBinding = binding;
			swapLabel.Visible = true;
		}
		else
		{
			if (selectedBinding != binding)
			{
				SwapKeys(binding, selectedBinding);
			}

			selectedBinding.UnSelect();
			binding.UnSelect();

			selectedBinding = null;
			swapLabel.Visible = false;
		}
	}
}
