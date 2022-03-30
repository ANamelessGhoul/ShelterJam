using Godot;
using Godot.Collections;
using System.Collections.Generic;
using GodotDictionary = Godot.Collections.Dictionary;

public class PatternPreview : Node2D
{
	[Export]
	public Color placeableColor = Colors.Blue;
	[Export]
	public Color nonPlaceableColor = Colors.Red;

	public bool IsShowing { get; private set; } = false;
	
	private int _rotations = 0;
	public int QuarterRotations 
	{ 
		get => _rotations;
		private set 
		{
			while (value < 0)
				value += 4;
			value %= 4;
			_rotations = value;
		} 
	}

	private Sprite _targetSprite;
	private Node2D _targetParent;

	public override void _Ready()
	{
		_targetSprite = GetNode<Sprite>("PatternSprite");
		_targetParent = GetNode<Node2D>("PatternParent");

	}

	public void RotateClockwise(int quarters)
	{
		QuarterRotations += quarters;
	}

	public void ShowPreview(Resource deathPattern, GameSpace space, Vector2 mapIndex)
	{
		if (deathPattern == null)
		{
			GD.PrintErr($"No Death Pattern set in {Name}");
			return;
		}

		var rotatedSkills = deathPattern.Call("get_skills_rotated", QuarterRotations, mapIndex) as GodotDictionary;

		foreach (string skill in rotatedSkills.Keys) 
		{
			var skillPositions = rotatedSkills[skill] as Array;
			foreach (Vector2 position in skillPositions)
			{
				PlacePreviewAt(space, skill, position);
			}
		}

		IsShowing = true;
	}

	public void PlacePreviewAt(GameSpace space, string tileType, Vector2 position) 
	{
		var targetPosition = space.WalkableMap.GetWorldPosition(position);
		var target = _targetSprite.Duplicate() as Sprite;

		var isOnObstruction = space.ObstructionMap.TileExistsAt(position);
		var isOnWalkable = space.WalkableMap.TileExistsAt(position);
		var isPlaceable = !isOnObstruction && isOnWalkable;

		target.Modulate = isPlaceable ? placeableColor : nonPlaceableColor;
		target.Visible = true;
		_targetParent.AddChild(target);
		target.GlobalPosition = targetPosition;
	}

	public void HidePreview()
	{
		foreach (Node2D child in _targetParent.GetChildren())
		{
			child.QueueFree();
		}

		IsShowing = false;
	}
}
