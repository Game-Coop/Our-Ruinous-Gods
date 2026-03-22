using Godot;
using System;

public partial class OnAnimationEventAudio : AudioBehavior
{
    [Export] public string AnimationName;

    protected override void Setup()
    {
        var animPlayer = FindParentOfType<AnimationPlayer>();
        
        if (animPlayer == null) return;

        animPlayer.AnimationFinished += name =>
        {
            if (name == AnimationName)
            {
                Play(animPlayer);
            }
        };
    }
}
