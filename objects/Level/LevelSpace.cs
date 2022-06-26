using Godot;
using System;
using System.Collections.Generic;

public class LevelSpace : Node2D
{
	[Signal]
	public delegate void LevelWon();

	[Signal]
	public delegate void ResetLevel();

	public Vector2 flagIndex;

	public CharacterSelection CharacterSelection { get; private set; }
	public ObstructionMap ObstructionMap { get; private set; }
	public SkillMap SkillMap { get; private set; }
    public Map WalkableMap { get; private set; }
	public Map VisualMap { get; private set; }
    public EnergyHandler EnergyHandler { get; private set; }

	public SkillApplier SkillApplier { get; private set; }
	public WalkPreview WalkPreview { get; private set; }
	public ErrorText ErrorText { get; private set; }

	public override void _Ready()
	{
		this.GetSingleton<GameManager>().State = GameManager.GameState.OnGoing;
		CharacterSelection = GetNode<CharacterSelection>("CharacterSelection");
		ObstructionMap = GetNode<ObstructionMap>("ObstructionMap");
		SkillMap = GetNode<SkillMap>("SkillMap");
		WalkableMap = GetNode<Map>("WalkableMap");
		VisualMap = GetNode<Map>("VisualMap");
		EnergyHandler = GetNode<EnergyHandler>("EnergyHandler");
		SkillApplier = GetNode<SkillApplier>("SkillApplier");
		WalkPreview = GetNode<WalkPreview>("WalkPreview");
		ErrorText = GetNode<ErrorText>("CanvasLayer/ErrorText");
	}

	public void SpendEnergy(int skillCost)
	{
		EnergyHandler.SpendEnergy(skillCost);
	}


	public bool IsPositionOverlappingWithCharacters(Vector2 position)
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
			if (!IsInstanceValid((Godot.Object)nodeChild))
				continue;

			if (nodeChild is Character character)
				characters.Add(character);
		}
		return characters;
	}

	public void PlayerMovedTo(Vector2 mapIndex)
	{
		if (mapIndex == flagIndex) 
		{
			EmitSignal(nameof(LevelWon));
		}
	}

	public void _on_ResetButton_pressed() 
	{
		EmitSignal(nameof(ResetLevel));
	}
}
