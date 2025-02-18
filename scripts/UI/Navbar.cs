using System;
using System.Collections.Generic;
using Godot;

public class Navbar : Control
{
	private List<Pagination> paginations = new List<Pagination>();
	public event Action<int, int> OnNavigate;
	[Export] private NodePath paginationsPath;
	[Export] private NodePath leftButtonPath;
	[Export] private NodePath rightButtonPath;
	private Node paginationContainer;
	private Button leftButton;
	private Button rightButton;
	private int index;
	private bool isReady;
	public override void _Ready()
	{
		base._Ready();
		Init();
	}
	public Pagination AddPagination(PackedScene paginationTemplate)
	{
		Init();

		var pagination = paginationTemplate.Instance() as Pagination;
		pagination.OnClick += () => Select(index, pagination.GetIndex());

		paginationContainer.AddChild(pagination);
		paginationContainer.MoveChild(pagination, paginationContainer.GetChildCount());
		paginations.Add(pagination);
		if(paginations.Count == 1)
			pagination.Select();
		 else
			pagination.UnSelect();

		HandleVisuals();

		return pagination;
	}
	public void Clear()
	{
		Init();
		index = 0;
		foreach (var pagination in paginations)
		{
			pagination.QueueFree();
		}
		paginations.Clear();
	}
	public void NavigateTo(int toIndex)
	{
		var fromIndex = index;
		NavigateWithoutNotify(toIndex);
		OnNavigate?.Invoke(fromIndex, toIndex);
	}
	public void NavigateWithoutNotify(int toIndex)
	{
		Init();
		if (index >= paginations.Count)
		{
			GD.PrintErr("There is nothing to navigate to!");
			return;
		}
		paginations[index].UnSelect();
		paginations[toIndex].Select();
		index = toIndex;
		HandleVisuals();
	}
	private void Init()
	{
		if(isReady) return;
		isReady = true;
		
		paginationContainer = GetNode(paginationsPath);
		leftButton = GetNode<Button>(leftButtonPath);
		rightButton = GetNode<Button>(rightButtonPath);

		leftButton.Connect("pressed", this, nameof(LeftButtonPressed));
		rightButton.Connect("pressed", this, nameof(RightButtonPressed));

		Clear();
		for (int i = 0; i < paginationContainer.GetChildCount(); i++)
		{
			var pagination = paginationContainer.GetChild(i) as Pagination;
			pagination.OnClick += () => Select(index, i);
			paginations.Add(pagination);

			if (i == 0)
				pagination.Select();
			else
				pagination.UnSelect();
		}
		HandleVisuals();
	}
	private void LeftButtonPressed()
	{
		if (index > 0)
		{
			Select(index, index - 1);
		}
	}
	private void RightButtonPressed()
	{
		if (index < paginations.Count - 1)
		{
			Select(index, index + 1);
		}
	}
	private void Select(int from, int to)
	{
		GD.Print("Selecting from: " + from + " to: " + to);
		index = to;
		paginations[from].UnSelect();
		paginations[to].Select();
		HandleVisuals();
		OnNavigate?.Invoke(from, to);
	}
	private void HandleVisuals()
	{
		leftButton.Disabled = index == 0;
		rightButton.Disabled = index >= (paginations.Count - 1);

		var leftButtonColor = leftButton.Modulate;
		leftButtonColor.a = leftButton.Disabled ? 0f : 1f;
		leftButton.Modulate = leftButtonColor;

		var rightButtonColor = rightButton.Modulate;
		rightButtonColor.a = rightButton.Disabled ? 0f : 1f;
		rightButton.Modulate = rightButtonColor;
	}
   
}
