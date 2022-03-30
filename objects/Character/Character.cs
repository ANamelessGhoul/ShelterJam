using Godot;
using System;
using System.Collections.Generic;

public class Character : Node2D
{
	[Signal]
	public delegate void Selected(Character character);

	[Export]
	private Resource pattern = null;
	[Export]
	public int SkillEnergyCost;
	public Vector2 MapIndex { get; private set; }
	public int SkillId { get; private set; }
	public bool IsShowingSkillPreview => _patternPreview.IsShowing;

	private bool _isSelected;

	private Sprite _sprite;
	private Sounds _sounds;
	private Map _map;  // Any map
	private GameSpace _gameSpace;
	private PatternPreview _patternPreview;
	private WalkHighlights _walkHighlights;

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
		_walkHighlights = GetNode<WalkHighlights>("WalkHighlights");
		_sprite = GetNode<Sprite>("Sprite");
		//_outline = GetNode<Sprite>("Sprite/Outline");
		_gameSpace = GetNode<GameSpace>("../..");

		_sprite.Material = (Material)_sprite.Material.Duplicate();

		CallDeferred(nameof(GetMapIndex));
	}

	private void GetMapIndex()
	{
		_map = _gameSpace.VisualMap;
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
		var death = GetNode("Death") as Node2D;
		var deathAnimation = death.GetNode("AnimatedSprite") as AnimatedSprite;
		deathAnimation.Playing = true;
		deathAnimation.Visible = true;
		var timer = death.GetNode("Timer") as Timer;
		RemoveChild(death);
		GetParent().AddChild(death);
		death.GlobalPosition = GlobalPosition;
		timer.Start();
		QueueFree();
	}

	public List<Vector2> GetRotatedIndexes()
	{
		List<Vector2> indexes = _patternPreview.GetRotatedMapIndexes(pattern, MapIndex);
		return indexes;
	}

	public bool TryMoveToMapIndex(Vector2 targetMapIndex) 
	{
		if (!CanMoveToMapIndex(targetMapIndex))
			return false;

		// Move
		if (_gameSpace.SpeedupMap.TileExistsAt(targetMapIndex))
			_sounds.PlaySound("WalkSpedUp");
		else
			_sounds.PlaySound("Walk");

		MapIndex = targetMapIndex;
		GlobalPosition = _map.GetWorldPosition(MapIndex);
		return true;
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
		if (_gameSpace.SpeedupMap.TileExistsAt(MapIndex))
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
		(_sprite.Material as ShaderMaterial).SetShaderParam("intensity", 50);
		//_outline.Visible = true;
		_isSelected = true;
		ShowWalkHighlights();
	}

	public void Deselect() 
	{
		HidePatternPreview();
		_isSelected = false;
		(_sprite.Material as ShaderMaterial).SetShaderParam("intensity", 0);
		//_outline.Visible = false;
		HideWalkHighlights();
	}

	public void ShowWalkHighlights() 
	{
		var availableDirections = new bool[4];
		for (int i = 0; i < directions.Length; i++)
		{
			var target = directions[i] + MapIndex;
			availableDirections[i] = CanMoveToMapIndex(target);
			
		}

		_walkHighlights.ShowSides(availableDirections);
	}

	public void HideWalkHighlights()
	{
		_walkHighlights.HideSides();
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
}
