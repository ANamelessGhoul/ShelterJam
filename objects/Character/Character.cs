using Godot;
using System;
using System.Collections.Generic;

public class Character : Node2D
{
	[Signal]
	public delegate void Selected(Character character);

	[Export]
	private Resource pattern;
	[Export]
	public int SkillEnergyCost;
	public Vector2 MapIndex { get; private set; }
	public int SkillId { get; private set; }
	public bool IsShowingSkillPreview => _patternPreview.IsShowing;

	private bool _isSelected;

	private Sprite _sprite;
	private Sprite _outline;
	private Sounds _sounds;
	private Map _map;  // Any map
	private GameSpace _gameSpace;
	private PatternPreview _patternPreview;

	public override void _Ready()
	{
		_sounds = this.GetSingleton<Sounds>();
		_patternPreview = GetNode<PatternPreview>("PatternPreview");
		_sprite = GetNode<Sprite>("Sprite");
		_outline = GetNode<Sprite>("Sprite/Outline");
		_gameSpace = GetNode<GameSpace>("../..");

		//_sprite.ZIndex = (int)Position.y;

		CallDeferred(nameof(GetMapIndex));
	}

    private void GetMapIndex()
	{
		_map = _gameSpace.VisualMap;
		MapIndex = _map.GetMapIndex(GlobalPosition);
	}

    public override void _Process(float delta)
    {
		if (_isSelected)
			ProcessSkillRotation();

	}
	private void ProcessSkillRotation()
	{
		if (!_patternPreview.IsShowing)
			return;

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

	public void DieOnSkillCast()
    {
		QueueFree();
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

		if (_gameSpace.ObstructionMap.TileExistsAt(targetMapIndex))
			return false;

		if (!_gameSpace.SpeedupMap.TileExistsAt(targetMapIndex))
        {

        }

		// Move
		_sounds.PlaySound("Walk");
		MapIndex = targetMapIndex;
		GlobalPosition = _map.GetWorldPosition(MapIndex);
		//_sprite.ZIndex = (int)Position.y;
		return true;
	}

	public void Select()
	{
		EmitSignal(nameof(Selected), this);
		_sprite.SelfModulate = Colors.DarkGray;
		_outline.Visible = true;
		_isSelected = true;
	}

	public void Deselect() 
	{
		_sprite.SelfModulate = Colors.White;
		HidePatternPreview();
		_isSelected = false;
		_outline.Visible = false;

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
