using Godot;
using System;

public class ErrorText : Node
{
    private PackedScene _errorLabelScene;
    private Timer _timer;
    private bool _enabled = true;
    private string _lastError = "";
    
    public override void _Ready()
    {
        _errorLabelScene = GD.Load<PackedScene>("res://objects/ErrorText/ErrorLabel.tscn");
        _timer = GetNode<Timer>("Timer");
        _timer.Connect("timeout", this, nameof(OnTimerTimeout));
    }

    public void ShowErrorTextAtPosition(string errorText, Vector2 worldPosition)
    {
        if (!_enabled && errorText == _lastError)
            return;

        var errorLabel = _errorLabelScene.Instance<Label>();
        errorLabel.Text = errorText;
        AddChild(errorLabel);
        errorLabel.RectGlobalPosition = worldPosition - errorLabel.RectSize / 2;

        _lastError = errorText;
        _enabled = false;
        _timer.Start();
    }

    private void OnTimerTimeout()
    {
        _enabled = true;
    }
}
