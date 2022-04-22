using Godot;
using System;

public class Corpse : Node2D
{
	public bool IsActive { get; private set; } = false;
    public Vector2 MapIndex { get; private set; } = Vector2.Zero;


	private Map _map = null;
	private Character _livingForm;
	private AnimatedSprite _animation;

	public override void _Ready() 
	{
		_animation = GetNode<AnimatedSprite>("AnimatedSprite");

	}

	public override void _Process(float delta)
	{
		// TODO: Debug key remove later
		if (Visible && Input.IsActionJustPressed("ui_home")) 
		{
			Revive();
		}

	}

	public void Initialize(Character livingForm, Map map)
	{
		IsActive = true;
		Visible = true;
		_livingForm = livingForm;
		MapIndex = _livingForm.MapIndex;
		_map = map;
		UpdatePosition();
		ShowAnimation();
	}

	public void Revive()
	{
		if (!_animation.IsConnected("animation_finished", this, nameof(ReviveAnimCallback)))
			_animation.Connect("animation_finished", this, nameof(ReviveAnimCallback), flags:(uint) ConnectFlags.Oneshot);
		_animation.Play("Revive");
	}

	public void ReviveAnimCallback() 
	{
		IsActive = false;
		Visible = false;
		Node parent = GetParent();
		parent.AddChild(_livingForm);
		parent.RemoveChild(this);
		_livingForm.AddChild(this);

		_livingForm.WarpToMapIndex(MapIndex);
		_livingForm.Revive();
	}

	public void UpdatePosition()
	{
		GlobalPosition = _map.GetWorldPosition(MapIndex);
	}

	public void ShowAnimation()
	{
		_animation.Play("Die");
	}

}
