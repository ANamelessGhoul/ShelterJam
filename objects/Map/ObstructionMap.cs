using Godot;

public class ObstructionMap : Map
{
    public override void _Ready()
    {
        base._Ready();

        var placeholderId =  TileSet.FindTileByName("Placeholder");
        var invisibleId =  TileSet.FindTileByName("Invisible");

        var placeholders = GetUsedCellsById(placeholderId);
        foreach (Vector2 placeholderPos in placeholders)
        {
            SetCellv(placeholderPos, invisibleId);
        }
    }


}
