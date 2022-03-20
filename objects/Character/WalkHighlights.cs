using Godot;
using System;
using System.Collections.Generic;

public class WalkHighlights : Node2D
{
    [Export]
    public Color walkableColor = Colors.Blue;
    [Export]
    public Color nonWalkableColor = Colors.Red;

    public enum Side
    {
        TopRight,
        BottomRight,
        BottomLeft,
        TopLeft,
    }

    private Sprite[] _sprites = null;

    public override void _Ready()
    {
        _sprites = new[]{
            GetNode<Sprite>("TopRight"),
            GetNode<Sprite>("BottomRight"),
            GetNode<Sprite>("BottomLeft"),
            GetNode<Sprite>("TopLeft"),
        };
    }

    public void ShowSides(bool[] sidesToEnable) 
    {
        for (int i = 0; i < sidesToEnable.Length; i++)
        {
            _sprites[i].Visible = true;

            _sprites[i].Modulate = sidesToEnable[i] ? walkableColor : nonWalkableColor;
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
