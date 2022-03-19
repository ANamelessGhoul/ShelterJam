using Godot;
using System;
public class GameSpace : Node2D
{
	public CharacterSelection CharacterSelection { get; private set; }
	public Map Map { get; private set; }

	public override void _Ready()
	{
		Map = GetNode<Map>("TileMap");
		CharacterSelection = GetNode<CharacterSelection>("CharacterSelection");
	}

}
