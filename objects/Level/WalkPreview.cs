using Godot;
using System;
using System.Collections.Generic;

public class WalkPreview : Node
{
	private LevelSpace _levelSpace;
	private Dictionary<Character, List<Sprite>> _previewTiles = new Dictionary<Character, List<Sprite>>();

	public enum WalkableState
	{
		Valid,
		Invalid
	}

	public class WalkablePreviewTile 
	{
		public Vector2 Position { get; set; }
		public WalkableState State { get; set; }

		public WalkablePreviewTile() { }
		public WalkablePreviewTile(Vector2 position, WalkableState state)
		{
			Position = position;
			State = state;
		}
	}



	public override void _Ready()
	{
		_levelSpace = GetParent<LevelSpace>();

	}

	public void ShowPreview(Character owner, List<WalkablePreviewTile> tiles) 
	{
		// Refresh preview if already open
		if (_previewTiles.ContainsKey(owner))
			HidePreview(owner);

		var _map = _levelSpace.ObstructionMap;
		var addedSprites = new List<Sprite>();
		foreach (WalkablePreviewTile tile in tiles) 
		{
			var sprite = InstanceTile(tile.State.ToString());
			sprite.Position = _map.GetWorldPosition(tile.Position);
			_map.AddChild(sprite);
			addedSprites.Add(sprite);
		}

		_previewTiles.Add(owner, addedSprites);
	}

	public void HidePreview(Character owner) 
	{
		// Cancel if already not showing
		if (!_previewTiles.ContainsKey(owner))
			return;

		foreach (Sprite sprite in _previewTiles[owner]) 
		{
			sprite.QueueFree();
		}
		_previewTiles.Remove(owner);
	}

	public void HideAll() 
	{
		foreach (var spriteList in _previewTiles.Values) 
		{
			foreach (Sprite sprite in spriteList) 
			{
				sprite.QueueFree();
			}
		}
		_previewTiles.Clear();
	}

	private Sprite InstanceTile(string tileName) 
	{
		var template = GetNodeOrNull<Sprite>(tileName);
		if (template == null)
			throw new ArgumentException($"Tile with name {tileName} not found!");
		var sprite = (Sprite)template.Duplicate();
		sprite.Visible = true;
		return sprite;
	}
}
