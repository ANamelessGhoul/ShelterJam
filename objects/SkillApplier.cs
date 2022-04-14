using Godot;
using System;
using System.Collections.Generic;

public class SkillApplier : Node
{
	private LevelSpace _levelSpace;

	private Dictionary<string, Predicate<Vector2>> conditions = null;
	private Dictionary<string, Action<Vector2>> actions = null;

	public override void _Ready()
	{
		_levelSpace = GetParent<LevelSpace>();

		ReadyConditions();
		ReadyActions();
	}



	private void ReadyConditions()
	{
		conditions = new Dictionary<string, Predicate<Vector2>>
		{
			{ "Speedup",  IsTileWalkable },
			{ "Bounce",  IsTileWalkable },
			{ "WallBreak", (x) => _levelSpace.WalkableMap.TileExistsAt(x)}
		};
	}

	private void ReadyActions()
	{
		actions = new Dictionary<string, Action<Vector2>>
		{
			{ "Speedup", (x) => _levelSpace.SkillMap.SetTile(x, "Speedup")},
			{ "WallBreak", (x) => _levelSpace.ObstructionMap.SetCellv(x, -1)}
		};
	}

	public bool CanCastSkills(Godot.Collections.Dictionary skills)
	{
		foreach (string key in skills.Keys)
		{
			var IsConditionMet = GetSkillCondition(key);
			foreach (Vector2 position in skills[key] as Godot.Collections.Array)
			{
				if (!IsConditionMet(position))
				{
					return false;
				}
			}
		}

		return true;
	}

	public Predicate<Vector2> GetSkillCondition(string skill)
	{
		if (conditions == null)
		{
			GD.PrintErr("Conditions not generated yet!");
			return (x) => false;
		}

		if (!conditions.ContainsKey(skill))
		{
			return IsTileWalkable;
		}

		return conditions[skill];
	}

	public void CastSkill(string skill, Vector2 position) 
	{
		GetSkillAction(skill).Invoke(position);
	}

	public Action<Vector2> GetSkillAction(string skill)
	{
		if (actions == null)
		{
			GD.PrintErr("Actions not generated yet!");
			return (x) => { };
		}

		if (!actions.ContainsKey(skill))
		{
			return (x) =>
			{
				_levelSpace.SkillMap.SetTile(x, "Speedup");
				GD.PrintErr("Default action performed!");
			};
		}

		return actions[skill];
	}

	public bool TryApplyCharactersSkill(Character selectedCharacter)
	{
		int skillEnergyCost = selectedCharacter.SkillEnergyCost;
		if (skillEnergyCost > _levelSpace.EnergyHandler.Energy)
			return false;

		// Apply Cast
		var rotatedSkills = selectedCharacter.GetRotatedSkills();
		if (!CanCastSkills(rotatedSkills))
			return false;

		_levelSpace.SpendEnergy(skillEnergyCost);

		foreach (string key in rotatedSkills.Keys)
		{
			foreach (Vector2 position in rotatedSkills[key] as Godot.Collections.Array)
			{
				CastSkill(key, position);
			}
		}

		selectedCharacter.DieOnSkillCast();
		return true;
	}

	public bool IsTileWalkable(Vector2 position)
	{
		var isOnObstruction = _levelSpace.ObstructionMap.TileExistsAt(position);
		var isOnWalkable = _levelSpace.WalkableMap.TileExistsAt(position);
		return !isOnObstruction && isOnWalkable;
	}
}
