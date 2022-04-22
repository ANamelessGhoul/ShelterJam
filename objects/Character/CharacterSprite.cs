using Godot;
using System;

public class CharacterSprite : AnimatedSprite
{
    public override void _Ready()
    {
        var x = GetTree().CreateTimer(GD.Randf());
        x.Connect("timeout", this, nameof(DelayTimerCallback));
        Stop();
    }

    public void DelayTimerCallback() 
    {
        Play();
    }

}
