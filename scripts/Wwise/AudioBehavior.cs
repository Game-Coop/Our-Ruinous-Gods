using Godot;
using System;

public abstract partial class AudioBehavior : Node
{
    [Export] public AudioStream Audio;
    [Export] public bool PlayAs3D = true;

    private AudioStreamPlayer3D player3D;
    private AudioStreamPlayer player2D;

    public override void _Ready()
    {
        if (Audio == null) return;

        if (PlayAs3D)
        {
            player3D = new AudioStreamPlayer3D();
            AddChild(player3D);
            player3D.Stream = Audio;
        }
        else
        {
            player2D = new AudioStreamPlayer();
            AddChild(player2D);
            player2D.Stream = Audio;
        }

            Setup();
    }

    protected abstract void Setup();

    protected void Play(Node emitter = null)
    {
        if (PlayAs3D)
        {
            player3D?.Play();
        }
        else
        {
            player2D?.Play();
        }
    }

    protected T FindParentOfType<T>() where T : class
    {
        Node current = GetParent();

        while(current != null)
        {
            if(current is T match)
            {
                return match;
            }

            current = current.GetParent();
        }

        return null;
    }
}
