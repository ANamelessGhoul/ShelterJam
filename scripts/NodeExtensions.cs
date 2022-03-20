using Godot;

public static class NodeExtensions
{
	public static T GetSingleton<T>(this Node node) where T : Node
	{
		return node.GetNode<T>("/root/" + typeof(T).Name);
	}
}
