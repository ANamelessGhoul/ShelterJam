using Godot;
using Godot.Collections;

public class PatternPreview : Node2D
{
	public bool IsShowing { get; private set; } = false;

    private Sprite _targetSprite;
    private Node2D _targetParent;

    public override void _Ready()
    {
        _targetSprite = GetNode<Sprite>("PatternSprite");
        _targetParent = GetNode<Node2D>("PatternParent");
    }



	public void ShowPreview(Resource deathPattern, Map map, Vector2 mapIndex)
	{
		if (deathPattern == null)
		{
			GD.PrintErr($"No Death Pattern set in {Name}");
			return;
		}

		var positions = deathPattern.Get("positions") as Array;
		var origin = (Vector2)deathPattern.Get("origin");

		foreach (Vector2 position in positions)
		{
			var targetPosition = map.GetWorldPosition(mapIndex + position - origin);
			var target = _targetSprite.Duplicate() as Sprite;
			target.Visible = true;
			_targetParent.AddChild(target);
			target.GlobalPosition = targetPosition;
		}

		IsShowing = true;
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
