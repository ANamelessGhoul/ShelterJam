using Godot;
using System;
using System.Collections.Generic;

public class GameSpace : Node2D
{


	public CharacterSelection CharacterSelection { get; private set; }
	public Map ObstructionMap { get; private set; }
	public Map SpeedupMap { get; private set; }
	public Map VisualMap { get; private set; }
	public EnergyHandler EnergyHandler { get; private set; }

	public override void _Ready()
	{
		this.GetSingleton<GameManager>().State = GameManager.GameState.OnGoing;
		CharacterSelection = GetNode<CharacterSelection>("CharacterSelection");
		ObstructionMap = GetNode<Map>("ObstructionMap");
		SpeedupMap = GetNode<Map>("SpeedupMap");
		VisualMap = GetNode<Map>("VisualMap");
		EnergyHandler = GetNode<EnergyHandler>("EnergyHandler");
	}

	public void SpendEnergy(int skillCost)
	{
		EnergyHandler.SpendEnergy(skillCost);
	}


	public bool CheckIfOverlappingWithCharacters(Vector2 position)
	{
		foreach (Character character in GetAllCharacters())
		{
			if (position == character.MapIndex)
				return true;
		}

		return false;
	}

	public List<Character> GetAllCharacters()
    {
		List<Character> characters = new List<Character>();
		foreach (var nodeChild in ObstructionMap.GetChildren())
        {
			if (nodeChild is Character character)
				characters.Add(character);
        }
		return characters;
	}

}
