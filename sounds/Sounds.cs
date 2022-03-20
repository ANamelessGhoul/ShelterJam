using Godot;
using System;

public class Sounds : Node
{
    public void PlaySound(string soundName)
    {
        var a = GetNode<AudioStreamPlayer>(soundName);
        a.Play();
    }

    internal void StopPlaying(string soundName)
    {
        var a = GetNode<AudioStreamPlayer>(soundName);
        a.Stop();
    }

    internal void StartPlaying(string soundName)
    {
        // Use this for looping sounds only
        PlaySound(soundName);
    }
}
