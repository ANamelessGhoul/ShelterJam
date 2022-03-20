using Godot;
using System;

public class EnergyHandler : Node
{
    [Export]
    public int Energy { get; private set; }

    private GameManager _gameManager;
    
    
    public override void _Ready()
    {
        _gameManager= this.GetSingleton<GameManager>();
    }

    public void SpendEnergy(int skillCost)
    {
        Energy -= skillCost;
        GD.Print("Energy Left: " + Energy);
        Energy = Math.Max(Energy, 0); // clamp

        if (Energy == 0)
            _gameManager.State = GameManager.GameState.End;
    }
}
