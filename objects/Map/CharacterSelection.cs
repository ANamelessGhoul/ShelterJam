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
			int skillEnergyCost = _selectedCharacter.SkillEnergyCost;
			if (skillEnergyCost > _gameSpace.EnergyHandler.Energy) 
			{
				_sounds.PlaySound("CastingSpellFailed");
				return;
			}

			// Apply Cast
			var rotatedSkills = _selectedCharacter.GetRotatedSkills();
			var positionsToApplySkill = new List<Vector2>();
			foreach (string key in rotatedSkills.Keys) 
			{
				foreach (Vector2 position in rotatedSkills[key] as Godot.Collections.Array) 
				{
					positionsToApplySkill.Add(position);
				}
			}
			var canCastSkill = true;
			foreach (var position in positionsToApplySkill)
			{
				var isOnObstruction = _gameSpace.ObstructionMap.TileExistsAt(position);
				var isOnWalkable = _gameSpace.WalkableMap.TileExistsAt(position);
				if (isOnObstruction || !isOnWalkable) 
				{
					canCastSkill = false;
					break;
				}
			}



			if (canCastSkill) 
			{
				// Cast the skill, actually
				_sounds.PlaySound("CastSpell");
				_sounds.StopPlaying("Elinde_Ates_Var");
				_gameSpace.SpendEnergy(skillEnergyCost);
				_gameSpace.SpeedupMap.SetTiles(positionsToApplySkill, _selectedCharacter.SkillId);
				_selectedCharacter.DieOnSkillCast();
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

		var moveEnergyCost = _gameSpace.SpeedupMap.TileExistsAt(_selectedCharacter.MapIndex) ? 0 : 1;
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
