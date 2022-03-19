using Godot;
using Godot.Collections;

public class Character : Node2D
{
	[Signal]
	public delegate void Selected(Character character);

	[Export]
	private Resource pattern;
	public Vector2 MapIndex { get; private set; }

	private Map _map;
	private PatternPreview _patternPreview;

	public override void _Ready()
	{
		_patternPreview = GetNode<PatternPreview>("PatternPreview");

		_map = GetParent<Map>();
		MapIndex = _map.GetMapIndex(GlobalPosition);
	}

	public bool TryMoveToMapIndex(Vector2 mapIndex) 
	{
		if (mapIndex.DistanceSquaredTo(MapIndex) > 1)
			return false;

		if (!_map.IsTileWalkable(mapIndex))
			return false;

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

	public void TogglePatternPreview()
	{
		if (_patternPreview.IsShowing)
			HidePatternPreview();
		else
			ShowPatternPreview();
	}

	public void ShowPatternPreview() 
	{
		_patternPreview.ShowPreview(pattern, _map, MapIndex);
	}

	public void HidePatternPreview() 
	{
		_patternPreview.HidePreview();
	}

	private void _on_Area2D_Clicked() 
	{
		CallDeferred(nameof(Select));
	}
}
