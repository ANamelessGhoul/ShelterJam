using Godot;
using System;

public class CharacterSelection : Node2D
{
	private GameSpace _gameSpace;
	private Character _selectedCharacter = null;

	public override void _Ready() 
	{
		_gameSpace = GetParent<GameSpace>();
	}

    public override void _UnhandledInput(InputEvent @event)
    {
		if (Input.IsActionJustPressed("click"))
		{
			OnClick();
		}
	}
    public override void _Process(float delta)
    {

		if (Input.IsActionJustPressed("cancel_select"))
		{
			DeselectCharacter();
		}
		else if (Input.IsActionJustPressed("use_skill")) 
		{
			if (_selectedCharacter != null) 
			{
				_selectedCharacter.TogglePatternPreview();
			}
		}
    }
	public void SelectCharacter(Character character)
	{
		DeselectCharacter();
		_selectedCharacter = character;
	}

	public void DeselectCharacter()
	{
		if (_selectedCharacter == null)
			return;

		if (!IsInstanceValid(_selectedCharacter))
			return;

		_selectedCharacter.Deselect();
		_selectedCharacter = null;
	}

	private void OnClick()
	{
		if (_selectedCharacter == null)
			return;

		if (!IsInstanceValid(_selectedCharacter))
			return;

		if (_selectedCharacter.IsShowingSkillPreview)
		{
            int skillEnergyCost = _selectedCharacter.SkillEnergyCost;
			if (skillEnergyCost > _gameSpace.EnergyHandler.Energy) 
			{
				GD.Print("Not enough energy to cast");
				return;
			}

			_gameSpace.SpendEnergy(skillEnergyCost);
			var positionsToApplySkill = _selectedCharacter.GetRotatedIndexes();
			_gameSpace.SpeedupMap.SetTiles(positionsToApplySkill, _selectedCharacter.SkillId);
			_selectedCharacter.DieOnSkillCast();
			return;
		}

		var mapIndex = _gameSpace.ObstructionMap.WorldToMap(GetGlobalMousePosition());
		if (mapIndex == _selectedCharacter.MapIndex)
			return;



		var moveEnergyCost = _gameSpace.SpeedupMap.TileExistsAt(_selectedCharacter.MapIndex) ? 0 : 1;
		var outOfEnergy = moveEnergyCost > _gameSpace.EnergyHandler.Energy;
		if (outOfEnergy || !_selectedCharacter.TryMoveToMapIndex(mapIndex))
		{
			if (outOfEnergy)
				GD.Print("Not enough energy to Move");
			DeselectCharacter();
			return;
		}

		_gameSpace.SpendEnergy(moveEnergyCost);
	}

    public void _on_Character_Selected(Character character) 
	{
		SelectCharacter(character);
	}

}
