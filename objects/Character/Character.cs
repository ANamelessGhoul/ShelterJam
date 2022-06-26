using Godot;
using System;
using System.Collections.Generic;
using GodotDictionary = Godot.Collections.Dictionary;

public class Character : Node2D
{
	[Signal]
	public delegate void Selected(Character character);

	[Export]
	private Resource pattern = null;
	[Export]
	public int SkillEnergyCost;
	public Vector2 MapIndex { get; private set; }
	public bool IsShowingSkillPreview => _patternPreview.IsShowing;

	private bool _isSelected;

	private CharacterSprite _sprite;
	private Sprite _selectionArrow;
	private Sounds _sounds;
	private Map _map;  // Any map
	private LevelSpace _gameSpace;
	private PatternPreview _patternPreview;

	private readonly Vector2[] directions = new[]
	{
		Vector2.Up,
		Vector2.Right,
		Vector2.Down,
		Vector2.Left,
	};

	public override void _Ready()
	{
		_sounds = this.GetSingleton<Sounds>();
		_patternPreview = GetNode<PatternPreview>("PatternPreview");
		_sprite = GetNode<CharacterSprite>("Sprite");
		_selectionArrow = GetNode<Sprite>("Arrow");
		_gameSpace = GetNode<LevelSpace>("../..");

		CallDeferred(nameof(GetMapIndex));
	}

    public void GetMapIndex()
	{
		_map = _gameSpace.ObstructionMap;
		MapIndex = _map.GetMapIndex(GlobalPosition);
		GlobalPosition = _map.GetWorldPosition(MapIndex);
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
			_sounds.PlaySound("RotateIndicator");
		}
		else if (Input.IsActionJustReleased("rotate_ccw"))
		{
			_patternPreview.RotateClockwise(-1);
			HidePatternPreview();
			ShowPatternPreview();
			_sounds.PlaySound("RotateIndicator");
		}
	}

	public void DieOnSkillCast()
	{
		_gameSpace.CharacterSelection.DeselectCharacter();
		var corpse = GetNode<Corpse>("Corpse");
		RemoveChild(corpse);
		GetParent().AddChild(corpse);
		corpse.Initialize(this, _map);
		GetParent().RemoveChild(this);
	}

	public void Revive()
	{
		HidePatternPreview();
	}

	public GodotDictionary GetRotatedSkills()
	{
		return pattern.Call("get_skills_rotated", _patternPreview.QuarterRotations, MapIndex) as GodotDictionary;
	}

	public bool TryMoveToMapIndex(Vector2 targetMapIndex) 
	{
		if (!CanMoveToMapIndex(targetMapIndex))
			return false;

		// Move
		if (_gameSpace.SkillMap.TileExistsAt(targetMapIndex))
			_sounds.PlaySound("WalkSpedUp");
		else
			_sounds.PlaySound("Walk");

		MapIndex = targetMapIndex;
		GlobalPosition = _map.GetWorldPosition(MapIndex);
		HideWalkHighlights();
		ShowWalkHighlights();
		return true;
	}

	public void WarpToMapIndex(Vector2 mapIndex) 
	{
		MapIndex = mapIndex;
		GlobalPosition = _map.GetWorldPosition(MapIndex);
	}

	public bool CanMoveToMapIndex(Vector2 targetMapIndex)
	{
		// Location blocked cheks
		if (targetMapIndex.DistanceSquaredTo(MapIndex) > 1)
			return false;

		if (_gameSpace.ObstructionMap.TileExistsAt(targetMapIndex))
			return false;

		if (!_gameSpace.WalkableMap.TileExistsAt(targetMapIndex))
			return false;

		if (_gameSpace.IsPositionOverlappingWithCharacters(targetMapIndex))
			return false;

		// Energy checks
		if (_gameSpace.SkillMap.TileExistsAt(MapIndex))
		{
			return true;
		}

		if (_gameSpace.EnergyHandler.Energy == 0)
			return false;

		
		return true;
	}

	public void ConnectSignal(string signalName, Godot.Object target, string methodName) 
	{
		Connect(signalName, target, methodName);
	}

	public void Select()
	{
		EmitSignal(nameof(Selected), this);
		_isSelected = true;
		_sprite.OutlineType = CharacterSprite.Outline.Full;
		_sprite.HasOutline = true;
		_selectionArrow.Visible = true;
		ShowWalkHighlights();
	}

	public void Deselect() 
	{
		HidePatternPreview();
		_isSelected = false;
		_sprite.HasOutline = false;
		_selectionArrow.Visible = false;
		HideWalkHighlights();
	}

	public void ShowWalkHighlights() 
	{
		var movements = new List<WalkPreview.WalkablePreviewTile>();
		for (int i = 0; i < directions.Length; i++)
		{
			var targetIndex = directions[i] + MapIndex;
			var validMove = CanMoveToMapIndex(targetIndex);
			var preview = new WalkPreview.WalkablePreviewTile()
			{
				Position = targetIndex,
				State = validMove ? WalkPreview.WalkableState.Valid : WalkPreview.WalkableState.Invalid
			};
			movements.Add(preview);
		}

		_gameSpace.WalkPreview.ShowPreview(this, movements);
	}

	public void HideWalkHighlights()
	{
		_gameSpace.WalkPreview.HidePreview(this);
	}

	public void TogglePatternPreview()
	{
		if (_patternPreview.IsShowing)
		{
			HidePatternPreview();
			ShowWalkHighlights();
		}
		else 
		{
			ShowPatternPreview();
			HideWalkHighlights();
		}
	}

	public void ShowPatternPreview() 
	{
		_patternPreview.ShowPreview(pattern, _gameSpace, MapIndex);
	}

	public void HidePatternPreview() 
	{
		_patternPreview.HidePreview();
	}

	private void _on_Area2D_Clicked() 
	{
		CallDeferred(nameof(Select));
	}

	private void _on_Input_HoverChanged(bool isHovering)
	{
		if (_isSelected)
			return;

		if (isHovering)
		{
			_sprite.OutlineType = CharacterSprite.Outline.Partial;
			_sprite.HasOutline = true;
		}
		else
		{
			_sprite.HasOutline = false;
		}
	}
}
