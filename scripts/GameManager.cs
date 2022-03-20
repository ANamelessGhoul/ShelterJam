using Godot;

public class GameManager : Node
{
	[Signal]
	public delegate void GameStateChanged(GameState gameState);

	public enum GameState
	{
		OnGoing, End
	}

	private GameState _state;

	public GameState State
	{
		get => _state;

		set
		{
			if (value == _state)
				return;
			
			_state = value;
			EmitSignal(nameof(GameStateChanged), _state);
		}
	}
}
