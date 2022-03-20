using Godot;
using System;

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

}
