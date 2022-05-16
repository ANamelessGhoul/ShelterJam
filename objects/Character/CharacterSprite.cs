using Godot;
using System;

public class CharacterSprite : AnimatedSprite
{
    private ShaderMaterial _material;
    public bool HasOutline 
    {
        set 
        {
            var intensity = value ? 50 : 0;
            _material.SetShaderParam("intensity", intensity);
        } 
    }
    public override void _Ready()
    {
        var x = GetTree().CreateTimer(GD.Randf());
        x.Connect("timeout", this, nameof(DelayTimerCallback));
        Stop();

        _material = (ShaderMaterial)Material;
    }

    public void DelayTimerCallback() 
    {
        Play();
    }

    public void ShowOutline() 
    {
    }

}
