using Godot;
using System;

public class Death : Node2D
{
    public void _on_Timer_timeout()
    {
        QueueFree();
    }
}
