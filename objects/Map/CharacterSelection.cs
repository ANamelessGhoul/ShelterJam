using Godot;
using System;
using System.Collections.Generic;

public class CharacterSelection : Node2D
{
	private Sounds _sounds;
	private LevelSpace _gameSpace;
	private Character _selectedCharacter = null;

	public override void _Ready()
	{
		_gameSpace = GetParent<LevelSpace>();
		_sounds = this.GetSingleton<Sounds>();

		ConnectPlayerSignals();
	}

	private void ConnectPlayerSignals()
	{
		GetTree().CallGroup("Character", "ConnectSignal", "Selected", this, "_on_Character_Selected");
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
			_sounds.StopPlaying("Elinde_Ates_Var");
			DeselectCharacter();
		}
		else if (Input.IsActionJustPressed("use_skill"))
		{
			_sounds.PlaySound("RotateIndicator");
			_sounds.StartPlaying("Elinde_Ates_Var");
			if (_selectedCharacter != null && IsInstanceValid(_selectedCharacter))
			{
				_selectedCharacter.TogglePatternPreview();
			}
		}
	}
	public void SelectCharacter(Character character)
	{
		DeselectCharacter();
		_selectedCharacter = character;
		_sounds.PlaySound("PawnSelect");
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
			bool didCast = _gameSpace.SkillApplier.TryApplyCharactersSkill(_selectedCharacter);

			if (didCast)
			{
				_sounds.PlaySound("CastSpell");
				_sounds.StopPlaying("Elinde_Ates_Var");
			}
			else
			{
				_sounds.PlaySound("CastingSpellFailed");
			}

			return;

		}

		var mapIndex = _gameSpace.ObstructionMap.WorldToMap(GetGlobalMousePosition());
		if (mapIndex == _selectedCharacter.MapIndex)
			return;

		var moveEnergyCost = _gameSpace.SkillMap.TileExistsAt(_selectedCharacter.MapIndex) ? 0 : 1;
		var outOfEnergy = moveEnergyCost > _gameSpace.EnergyHandler.Energy;
		if (outOfEnergy || !_selectedCharacter.TryMoveToMapIndex(mapIndex))
		{
			if (outOfEnergy)
				GD.Print("Not enough energy to Move");
			DeselectCharacter();
			return;
		}
		// Moved

		_gameSpace.PlayerMovedTo(mapIndex);

		_gameSpace.SpendEnergy(moveEnergyCost);
	}



	public void _on_Character_Selected(Character character)
	{
		SelectCharacter(character);
	}


}
