using Godot;
using System;

public class Map : TileMap
{
	private GameSpace _gameSpace;
	public Vector2 WorldOffset => new Vector2(0, CellSize.y / 2);


	public override void _Ready()
	{
		_gameSpace = GetParent<GameSpace>();
	}

	public Vector2 GetWorldPosition(Vector2 mapIndex) 
	{
		return MapToWorld(mapIndex) + WorldOffset; 
	}

	public Vector2 GetMapIndex(Vector2 worldPosition) 
	{
		return WorldToMap(worldPosition - WorldOffset);
	}

	public bool IsTileWalkable(Vector2 mapIndex) 
	{
		// TODO: Update at every turn instead of each query
		var usedCells = GetUsedCells();
		
		return !usedCells.Contains(mapIndex);
	}
}
