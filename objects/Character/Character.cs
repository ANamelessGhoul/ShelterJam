using Godot;
using Godot.Collections;

public class Character : Node2D
{
	[Signal]
	public delegate void Selected(Character character);

	[Export]
	private Resource pattern;
	public Vector2 MapIndex { get; private set; }

	private Sprite _targetSprite;

	private Map _map; 


	private Sounds _sounds;

	public override void _Ready()
	{
		_sounds = this.GetSingleton<Sounds>();
		_targetSprite = GetNode<Sprite>("TargetSprite");
		_map = GetParent<Map>();
		MapIndex = _map.GetMapIndex(GlobalPosition);
	}

	public bool TryMoveToMapIndex(Vector2 mapIndex) 
	{
		if (mapIndex.DistanceSquaredTo(MapIndex) > 1)
			return false;

		if (!_map.IsTileWalkable(mapIndex))
			return false;

		_sounds.PlaySound("Walk");
		MapIndex = mapIndex;
		GlobalPosition = _map.GetWorldPosition(MapIndex);
		return true;
	}

	public void Select()
	{
		EmitSignal(nameof(Selected), this);
		Modulate = Colors.DarkGray;
	}

	public void Deselect() 
	{
		Modulate = Colors.White;
	}

	public void ShowTargets() 
	{
		var positions = pattern.Get("positions") as Array;
		var origin = (Vector2) pattern.Get("origin");
		foreach (Vector2 position in positions) 
		{
			var targetPosition = _map.GetWorldPosition(MapIndex + position - origin);
			var target = _targetSprite.Duplicate() as Sprite;
			target.Visible = true;
			AddChild(target);
			target.GlobalPosition = targetPosition;
		}
	}

	public void HideTargets() 
	{
	}

	private void _on_Area2D_Clicked() 
	{
		CallDeferred(nameof(Select));
	}
}
