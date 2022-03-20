using Godot;
using System;

public class SceneManager : Node2D
{
	private int levelIndex = 1;

	private GameSpace activeLevel = null;

	public override void _Ready()
	{
		PackedScene x = (PackedScene)GD.Load($"res://scenes/Level1.tscn");
		activeLevel = x.Instance<GameSpace>();
		activeLevel.Connect("LevelWon", this, nameof(LoadNextLevel));
		AddChild(activeLevel);
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("debug_reset_level")) 
		{
			LoadLevel(levelIndex);
		}
		if (Input.IsActionJustPressed("debug_reset_game")) 
		{
		}
		if (Input.IsActionJustPressed("debug_close_game"))
		{
			GetTree().Quit();
		}
		if (Input.IsActionJustPressed("mute_sound")) { }
		if (Input.IsActionJustPressed("mute_music")) { }

	}

	public void LoadLevel(int level) 
	{
		levelIndex = level;
		activeLevel.QueueFree();
		PackedScene x = (PackedScene)GD.Load($"res://scenes/Level{level}.tscn");
		activeLevel = x.Instance<GameSpace>();
		activeLevel.Connect("LevelWon", this, nameof(LoadNextLevel));
		AddChild(activeLevel);
	}

	public void LoadNextLevel() 
	{
		LoadLevel(levelIndex);
	}
}
