using Godot;

public class CharacterInput : Area2D
{
	[Signal]
	public delegate void Clicked();

	[Signal]
	public delegate void HoverChanged(bool isHovering);

	public bool IsHovering { get; private set; }

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
				_isClicked = true;
			}
		}

		GetTree().SetInputAsHandled();
	}

}
