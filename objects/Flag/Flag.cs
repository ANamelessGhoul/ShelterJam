using Godot;
using System;

public class Flag : Sprite
{
	private LevelSpace _gameSpace;
	private Map _map;

	public override void _Ready()
	{
		_gameSpace = GetNode<LevelSpace>("../..");

		_map = GetParent<Map>();
		var mapIndex = _map.GetMapIndex(GlobalPosition);
		GlobalPosition = _map.GetWorldPosition(mapIndex);

		_gameSpace.flagIndex = mapIndex;
	}
	
}
