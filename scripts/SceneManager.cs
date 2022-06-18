using Godot;
using System;

public class SceneManager : Node2D
{
	[Export]
	public int levelIndex = 1;

	private LevelSpace activeLevel = null;

	public override void _Ready()
	{
		LoadLevel(levelIndex);
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("debug_reset_level")) 
		{
			ResetCurrentLevel();
		}
		if (Input.IsActionJustPressed("debug_reset_game")) 
		{
			LoadLevel(1);
		}
		if (Input.IsActionJustPressed("debug_close_game"))
		{
			GetTree().Quit();
		}
		if (Input.IsActionJustPressed("mute_sound")) { }
		if (Input.IsActionJustPressed("mute_music")) { }
		
		// TODO: Next and Previous Level

	}

	public void LoadLevel(int level) 
	{
		levelIndex = level;
		if (activeLevel != null)
			activeLevel.QueueFree();
		PackedScene x = (PackedScene)GD.Load($"res://scenes/Level{level}.tscn");
		activeLevel = x.Instance<LevelSpace>();
		activeLevel.Connect("LevelWon", this, nameof(LoadNextLevel));
		activeLevel.Connect("ResetLevel", this, nameof(ResetCurrentLevel));
		AddChild(activeLevel);
	}

	public void LoadNextLevel() 
	{
		LoadLevel(levelIndex + 1);
	}

	public void ResetCurrentLevel() 
	{
		LoadLevel(levelIndex);
	}
}
