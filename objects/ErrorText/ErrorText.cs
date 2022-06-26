using Godot;
using System;

public class ErrorText : Node
{
    private PackedScene _errorLabelScene;
    
    public override void _Ready()
    {
        _errorLabelScene = GD.Load<PackedScene>("res://objects/ErrorText/ErrorLabel.tscn");
    }

    public void ShowErrorTextAtPosition(string errorText, Vector2 worldPosition)
    {
        var errorLabel = _errorLabelScene.Instance<Label>();
        errorLabel.Text = errorText;
        AddChild(errorLabel);
        errorLabel.RectGlobalPosition = worldPosition - errorLabel.RectSize / 2;
    }
}
