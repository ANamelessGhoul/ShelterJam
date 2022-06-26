using Godot;

public class CharacterInput : Area2D
{
	[Signal]
	public delegate void Clicked();

	[Signal]
	public delegate void HoverChanged(bool isHovering);

	private bool _isClicked = false;


	public override void _Process(float delta) 
	{
		ProcessClick();
	}

	private void ProcessClick() 
	{
		if (!_isClicked)
			return;

		CallDeferred(nameof(EmitClickedSignal));
		_isClicked = false;
	}

	public void EmitClickedSignal() 
	{
		EmitSignal(nameof(Clicked));
	}



	private void _on_Area2D_input_event(Node viewport, InputEvent inputEvent, int shapeIdx)
	{
		if (inputEvent is InputEventMouseButton mouseButtonInput) 
		{
			if (mouseButtonInput.IsPressed() && mouseButtonInput.ButtonIndex == (int)ButtonList.Left) 
			{
				_isClicked = true;
			}
		}

		GetTree().SetInputAsHandled();
	}

	private void _on_Input_mouse_entered()
	{
		EmitSignal(nameof(HoverChanged), true);
	}

	private void _on_Input_mouse_exited()
	{
		EmitSignal(nameof(HoverChanged), false);
	}
}
