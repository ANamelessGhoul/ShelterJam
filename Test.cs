using Godot;
using System;

public class Test : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private Sprite sprite;
	
	public override void _Init()
	{
		
	}
	
	// At scene enter
	public override void _Ready()
	{
		sprite = GetNode("Sprite");
	}
	
  public override void _Process(float delta)
  {
	sprite.scale = new Vector2(1,1 + delta*20);
	  
	
  }
	

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
