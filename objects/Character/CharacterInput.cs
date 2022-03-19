using Godot;
using System;

public class CharacterInput : Area2D
{
	[Signal]
	public delegate void Clicked();

	private void _on_Area2D_input_event(Node viewport, InputEvent inputEvent, int shapeIdx)
	{
		if (inputEvent is InputEventMouseButton mouseButtonInput) 
		{
			if (mouseButtonInput.IsPressed() && mouseButtonInput.ButtonIndex == (int)ButtonList.Left) 
			{
				EmitSignal(nameof(Clicked));
			}
		}
	}

}
