using Godot;
using Godot.Collections;

public class Character : Node2D
{
	[Signal]
	public delegate void Selected(Character character);

	[Export]
	private Resource pattern;
	public Vector2 MapIndex { get; private set; }

	private Sprite _sprite;
	private Sounds _sounds;
	private Map _map;
	private PatternPreview _patternPreview;

	public override void _Ready()
	{
		_sounds = this.GetSingleton<Sounds>();
		_patternPreview = GetNode<PatternPreview>("PatternPreview");
		_sprite = GetNode<Sprite>("Sprite");
		_map = GetParent<Map>();
		MapIndex = _map.GetMapIndex(GlobalPosition);
	}

    public override void _Process(float delta)
    {
		if (Input.IsActionJustReleased("rotate_cw"))
		{
			_patternPreview.RotateClockwise(1);
			HidePatternPreview();
			ShowPatternPreview();
		}
		else if (Input.IsActionJustReleased("rotate_ccw")) 
		{
			_patternPreview.RotateClockwise(-1);
			HidePatternPreview();
			ShowPatternPreview();
		}
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
		_sprite.SelfModulate = Colors.DarkGray;
	}

	public void Deselect() 
	{
		_sprite.SelfModulate = Colors.White;
		HidePatternPreview();
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
