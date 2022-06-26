using Godot;
using System;

public class ErrorText : Node
{
    private PackedScene _errorLabelScene;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _errorLabelScene = GD.Load<PackedScene>("res://objects/ErrorText/ErrorLabel.tscn");

        ShowErrorTextAtPosition("This is an Error Message!", new Vector2(120, 120));
    }

    public void ShowErrorTextAtPosition(string errorText, Vector2 worldPosition)
    {
        var errorLabel = _errorLabelScene.Instance<Label>();
        errorLabel.Text = errorText;
        AddChild(errorLabel);
        errorLabel.RectGlobalPosition = worldPosition - errorLabel.RectSize / 2;
    }
}
