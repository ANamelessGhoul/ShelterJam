using Godot;
using System;

public class CharacterSelection : Node
{
	private GameSpace _gameSpace;
	private Character _selectedCharacter = null;


	public override void _Ready() 
	{
		_gameSpace = GetParent<GameSpace>();
	}

	public void SelectCharacter(Character character) 
	{
		_selectedCharacter = character;
	}

	public void DeselectCharacter() 
	{
		_selectedCharacter = null;
	}

}
