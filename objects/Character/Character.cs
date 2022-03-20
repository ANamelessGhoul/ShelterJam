using Godot;
using System.Collections.Generic;

public class Character : Node2D
{
	[Signal]
	public delegate void Selected(Character character);

	[Export]
	private Resource pattern;
	public Vector2 MapIndex { get; private set; }
	public int SkillId { get; private set; }
	public bool IsShowingSkillPreview => _patternPreview.IsShowing;

	private Sprite _sprite;
	private Sounds _sounds;
	private Map _map;  // Any map
	private GameSpace _gameSpace;
	private PatternPreview _patternPreview;

	public override void _Ready()
	{
		_sounds = this.GetSingleton<Sounds>();
		_patternPreview = GetNode<PatternPreview>("PatternPreview");
		_sprite = GetNode<Sprite>("Sprite");
		_gameSpace = GetParent<GameSpace>();
		_map = _gameSpace.VisualMap;
		GD.Print(_gameSpace);
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

	public List<Vector2> GetRotatedIndexes()
    {
        List<Vector2> indexes = _patternPreview.GetRotatedMapIndexes(pattern, MapIndex);
		return indexes;
	}

    public bool TryMoveToMapIndex(Vector2 targetMapIndex) 
	{
		if (targetMapIndex.DistanceSquaredTo(MapIndex) > 1)
			return false;

		if (!_gameSpace.ObstructionMap.TileExistsAt(targetMapIndex))
			return false;

		if (_gameSpace.SpeedupMap.TileExistsAt(targetMapIndex))
        {

        }

		// Move
		_sounds.PlaySound("Walk");
		MapIndex = targetMapIndex;
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
