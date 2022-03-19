using Godot;
using System;

public class Character : Node2D
{
	private Map _map;
	private Vector2 _mapIndex;

	public override void _Ready()
	{
		_map = GetParent<Map>();
		_mapIndex = _map.GetMapIndex(GlobalPosition);
	}

	public override void _Process(float delta)
	{
		var movement = Vector2.Zero;
		if (Input.IsActionJustPressed("move_right"))
			movement = Vector2.Right;
		if (Input.IsActionJustPressed("move_left"))
			movement = Vector2.Left;



		if (movement != Vector2.Zero) 
		{
			var nextMapIndex = _mapIndex + movement;
			if (_map.IsTileWalkable(nextMapIndex)) 
			{
				_mapIndex = nextMapIndex;
				GlobalPosition = _map.GetWorldPosition(_mapIndex);
			}
		}
	}

	private void _on_Area2D_Clicked() 
	{
		GD.Print("Character Clicked!");
	}
}
