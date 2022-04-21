using Godot;
using System;

public class Corpse : Node2D
{
	private Vector2 _mapIndex = Vector2.Zero;
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
		Visible = true;
		_livingForm = livingForm;
		_mapIndex = _livingForm.MapIndex;
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
		Visible = false;
		Node parent = GetParent();
		parent.AddChild(_livingForm);
		parent.RemoveChild(this);
		_livingForm.AddChild(this);

		_livingForm.WarpToMapIndex(_mapIndex);
		_livingForm.Revive();
	}

	public void UpdatePosition()
	{
		GlobalPosition = _map.GetWorldPosition(_mapIndex);
	}

	public void ShowAnimation()
	{
		_animation.Play("Die");
	}

}
