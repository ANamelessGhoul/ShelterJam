using Godot;

public class GameSpace : Node2D
{
	public CharacterSelection CharacterSelection { get; private set; }
	public Map ObstructionMap { get; private set; }
	public Map SpeedupMap { get; private set; }

	public override void _Ready()
	{
		SpeedupMap = GetNode<Map>("SpeedupMap");
		ObstructionMap = GetNode<Map>("ObstructionMap");
		CharacterSelection = GetNode<CharacterSelection>("CharacterSelection");
	}

}
