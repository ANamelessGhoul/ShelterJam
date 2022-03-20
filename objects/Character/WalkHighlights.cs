using Godot;
using System;
using System.Collections.Generic;

public class WalkHighlights : Node2D
{
    public enum Side
    {
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft
    }

    private Sprite[] _sprites = null;

    public override void _Ready()
    {
        _sprites = new[]{
            GetNode<Sprite>("TopLeft"),
            GetNode<Sprite>("TopRight"),
            GetNode<Sprite>("BottomRight"),
            GetNode<Sprite>("BottomLeft")
        };
    }

    public void ShowSides(List<Side> sides) 
    {
        foreach (var side in sides)
        {
            _sprites[(int)side].Visible = true;
        }
    }

    public void HideSides() 
    {
        foreach (var sprite in _sprites)
        {
            sprite.Visible = false;
        }
    }

}
