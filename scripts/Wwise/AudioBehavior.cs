using Godot;
using System;

public abstract partial class AudioBehavior : Node
{
    [Export] public AudioEventData Audio;

    public override void _Ready()
    {
        if (Audio?.LoadBankOnReady == true && !string.IsNullOrEmpty(Audio.BankName))
        {
            WwiseManager.LoadBank(Audio.BankName);
        }

        Setup();
    }

    protected abstract void Setup();

    protected void Play(Node emitter = null)
    {
        if(Audio == null)
        {
            return;
        }

        WwiseManager.PostEvent(Audio.EventName, emitter ?? this);
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
