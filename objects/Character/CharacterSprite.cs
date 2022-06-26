using Godot;
using System;

public class CharacterSprite : AnimatedSprite
{
    public enum Outline
    {
        Partial,
        Full
    }
    
    private ShaderMaterial _material;
    public bool HasOutline 
    {
        set 
        {
            var intensity = value ? 50 : 0;
            _material.SetShaderParam("intensity", intensity);
        } 
    }

    private Outline _outlineType;

    public Outline OutlineType
    {
        get => _outlineType;
        set
        {
            switch (value)
            {
                case Outline.Partial:
                    _material.SetShaderParam("outline_color", Colors.Gray);
                    _material.SetShaderParam("outline_color_2", Colors.Gray);
                    break;
                case Outline.Full:
                    _material.SetShaderParam("outline_color", Colors.White);
                    _material.SetShaderParam("outline_color_2", Colors.White);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }

            _outlineType = value;
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
