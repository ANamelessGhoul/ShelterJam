using Godot;
using System;

public class CharacterSelection : Node2D
{
	private GameSpace _gameSpace;
	private Character _selectedCharacter = null;

	public override void _Ready() 
	{
		_gameSpace = GetParent<GameSpace>();
	}

    public override void _Process(float delta)
    {
		if (Input.IsActionJustPressed("click"))
		{
			OnClick();
		}
		else if (Input.IsActionJustPressed("cancel_select"))
		{
			DeselectCharacter();
		}
		else if (Input.IsActionJustPressed("use_skill")) 
		{
			if (_selectedCharacter != null) 
			{
				_selectedCharacter.TogglePatternPreview();
			}
		}
    }
	public void SelectCharacter(Character character)
	{
		_selectedCharacter = character;
	}

	public void DeselectCharacter()
	{
		if (_selectedCharacter == null)
			return;
		_selectedCharacter.Deselect();
		_selectedCharacter = null;
	}

	private void OnClick() 
	{
		if (_selectedCharacter == null)
			return;

		var mapIndex = _gameSpace.Map.WorldToMap(GetGlobalMousePosition());
		if (mapIndex == _selectedCharacter.MapIndex)
			return;

		if (!_selectedCharacter.TryMoveToMapIndex(mapIndex))
		{
			DeselectCharacter();
		}
	}

	public void _on_Character_Selected(Character character) 
	{
		_selectedCharacter = character;
	}

}
