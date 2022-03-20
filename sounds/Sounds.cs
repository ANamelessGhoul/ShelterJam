using Godot;
using System;

public class Sounds : Node
{
    public void PlaySound(string soundName)
    {
        var a = GetNode<AudioStreamPlayer>(soundName);
        a.Play();
    }
}
