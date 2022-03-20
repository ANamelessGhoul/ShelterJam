using Godot;
using System;

public class EnergyLabel : Label
{
	public void _on_EnergyHandler_EnergyChanged(int energy) 
	{
		Text = energy.ToString();
	}
}
