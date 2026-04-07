using System;
using Godot;

public static class NodeExtensions
{
    public static void Traverse<T>(this Node node, Action<T> action)
	{
		if (node is T target)
			action(target);

		foreach (Node child in node.GetChildren())
			Traverse(child, action);
	}
}