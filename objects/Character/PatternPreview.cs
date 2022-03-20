using Godot;
using Godot.Collections;
using System.Collections.Generic;

public class PatternPreview : Node2D
{
	[Export]
	public Color placeableColor = Colors.Blue;
	[Export]
	public Color nonPlaceableColor = Colors.Red;

	public bool IsShowing { get; private set; } = false;

    private Sprite _targetSprite;
    private Node2D _targetParent;

	private int rotations = 0;

    public override void _Ready()
    {
        _targetSprite = GetNode<Sprite>("PatternSprite");
        _targetParent = GetNode<Node2D>("PatternParent");

	}

	public void RotateClockwise(int quarters) 
	{
		rotations += quarters;
		while (rotations < 0)
			rotations += 4;
		rotations %= 4;

	}

	public void ShowPreview(Resource deathPattern, ObstructionMap map, Vector2 mapIndex)
	{
		if (deathPattern == null)
		{
			GD.PrintErr($"No Death Pattern set in {Name}");
			return;
		}

        var rotatedPositons = GetRotatedMapIndexes(deathPattern, mapIndex);

        foreach (Vector2 position in rotatedPositons)
		{
			var targetPosition = map.GetWorldPosition(position);
			var target = _targetSprite.Duplicate() as Sprite;

			target.Modulate = map.TileExistsAt(position) ? nonPlaceableColor : placeableColor;
			target.Visible = true;
			_targetParent.AddChild(target);
			target.GlobalPosition = targetPosition;
		}

		IsShowing = true;
	}

    public List<Vector2> GetRotatedMapIndexes(Resource deathPattern, Vector2 characterIndex)
    {
		var rotatedPositions = new List<Vector2>();
        var positions = deathPattern.Get("positions") as Array;
		var origin = (Vector2)deathPattern.Get("origin");

		foreach (Vector2 position in positions)
		{
			var targetOffset = position - origin;
			for (int i = 0; i < rotations; i++)
				targetOffset = new Vector2(-targetOffset.y, targetOffset.x);
			rotatedPositions.Add(targetOffset + characterIndex);
		}

		return rotatedPositions;
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
