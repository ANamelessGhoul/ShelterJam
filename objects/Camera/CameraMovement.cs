using Godot;
using System;

public class CameraMovement : Camera2D
{
    private bool _isMiddleMousePressed;
    
    public override void _Ready()
    {
        
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.ButtonIndex == (int)ButtonList.Middle)
            {
                _isMiddleMousePressed = mouseButtonEvent.Pressed;
            }
        }

        if (_isMiddleMousePressed && inputEvent is InputEventMouseMotion mouseMotionEvent)
        {
            Offset -= mouseMotionEvent.Relative;
            Offset = Offset.Round();
        }
    }
}
