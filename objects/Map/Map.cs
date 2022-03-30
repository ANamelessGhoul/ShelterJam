using Godot;
using System.Collections.Generic;

public class Map : TileMap
{
	public Vector2 WorldOffset => new Vector2(0, CellSize.y / 2);

	public virtual void SetTiles(List<Vector2> positions, int tileId)
	{
		foreach (Vector2 position in positions)
			SetCellv(position, tileId);
	}

	public virtual void SetTiles(Godot.Collections.Array positions, int tileId)
	{
		foreach (Vector2 position in positions)
			SetCellv(position, tileId);
	}

	public Vector2 GetWorldPosition(Vector2 mapIndex) 
	{
		return MapToWorld(mapIndex) + WorldOffset; 
	}

	public Vector2 GetMapIndex(Vector2 worldPosition) 
	{
		return WorldToMap(worldPosition - WorldOffset);
	}

	public bool TileExistsAt(Vector2 mapIndex) 
	{
		return GetCellv(mapIndex) != -1;
	}
}
