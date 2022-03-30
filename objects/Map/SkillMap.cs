using Godot;
using System;
using System.Collections.Generic;

public class SkillMap : Map
{
    public void ApplySkills(Godot.Collections.Dictionary skills) 
    {
        foreach (string skillName in skills.Keys) 
        {
            var tileId = TileSet.FindTileByName(skillName);
            var positions = skills[skillName] as Godot.Collections.Array;
            SetTiles(positions, tileId);
        }
    }

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

    public override void SetTiles(Godot.Collections.Array positions, int tileId)
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
