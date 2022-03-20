using Godot;

public class CharacterInput : Area2D
{
	[Signal]
	public delegate void Clicked();

	[Signal]
	public delegate void HoverChanged(bool isHovering);

	public bool IsHovering { get; private set; }

	private void _on_Input_mouse_entered() 
	{
		IsHovering = true;
		EmitSignal(nameof(HoverChanged), IsHovering);
	}

	private void _on_Input_mouse_exited() 
	{
		IsHovering = false;
		EmitSignal(nameof(HoverChanged), IsHovering);
	}

	private void _on_Area2D_input_event(Node viewport, InputEvent inputEvent, int shapeIdx)
	{
		if (inputEvent is InputEventMouseButton mouseButtonInput) 
		{
			if (mouseButtonInput.IsPressed() && mouseButtonInput.ButtonIndex == (int)ButtonList.Left) 
			{
				EmitSignal(nameof(Clicked));
			}
		}

		GetTree().SetInputAsHandled();
	}

}
