using Godot;
using System;
using System.Collections;

public class ErrorLabel : Label
{
    private Tween _tween;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _tween = GetNode<Tween>("Tween");
        _tween.Connect("tween_all_completed", this, nameof(OnTweenComplete));

        CallDeferred(nameof(StartTween));
    }

    public void StartTween()
    {
        _tween.InterpolateProperty(
            this, 
            "rect_position",
            RectPosition,
            RectPosition + new Vector2(0, 100),
            4f,
            Tween.TransitionType.Cubic,
            Tween.EaseType.In,
            1f
        );
        
        _tween.InterpolateProperty(
            this,
            "self_modulate",
            SelfModulate,
            Colors.Transparent,
            1.5f,
            Tween.TransitionType.Cubic,
            Tween.EaseType.In,
            2f
        );
        _tween.Start();
    }

    private void OnTweenComplete()
    {
        QueueFree();
    }
}
