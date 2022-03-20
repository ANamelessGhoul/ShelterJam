using Godot;
using System;
using System.Collections.Generic;

public class SkillMap : Map
{
    public override void SetTiles(List<Vector2> positions, int tileId)
    {
        foreach (Vector2 position in positions) 
        {
            var oldCell = GetCellv(position);
            if (oldCell == -1)
                SetCellv(position, tileId);
            else
                SetCellv(position, -1);
        }

    }
}
