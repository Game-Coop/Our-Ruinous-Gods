using Godot;
using System;

public enum StateAudioTrigger
{
    Activated,
    Deactivated
}

public partial class OnStateChangedAudio : AudioBehavior
{
    [Export] public StateAudioTrigger Trigger;

    protected override void Setup()
    {
        /*var state = GetParent<State>();
        if(state == null) 
        { 
            return;
        }

        state.OnStateChanged += active =>
        {
            if (active && Trigger == StateAudioTrigger.Activated)
            {
                Play();
            }
            else if (!active && Trigger == StateAudioTrigger.Deactivated)
            {
                Play();
            }
        };*/
    }
}
