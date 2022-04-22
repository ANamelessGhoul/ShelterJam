using Godot;

public static class ReviveSkill
{
	public static bool CanCast(SceneTree tree, Vector2 position) 
	{
		var corpses = tree.GetNodesInGroup("Corpse");

		foreach (var node in corpses) 
		{
			if (node is Corpse corpse) 
			{
				if (corpse.IsActive && corpse.MapIndex == position) 
				{
					return true;
				}
			}
		}

		return false;
	}

	public static void Cast(SceneTree tree, Vector2 position) 
	{
		var corpses = tree.GetNodesInGroup("Corpse");
		foreach (var node in corpses)
		{
			if (node is Corpse corpse)
			{
				if (corpse.IsActive && corpse.MapIndex == position)
				{
					corpse.Revive();
				}
			}
		}
	}
}
