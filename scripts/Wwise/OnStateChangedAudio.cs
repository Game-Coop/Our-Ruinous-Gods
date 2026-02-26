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
    [Export] public string RequiredGroup = "player";

    protected override void Setup()
    {
        var area = FindParentOfType<Area3D>();
        if (area == null)
        {
            return;
        }

        area.BodyEntered += body =>
        {
            if (!IsValidBody(body))
            {
                return;
            }

            if (Trigger == StateAudioTrigger.Activated)
            {
                Play(GetParent());
            }
        };

        area.BodyExited += body =>
        {
            if (!IsValidBody(body))
            {
                return;
            }
            if (Trigger == StateAudioTrigger.Deactivated)
            {
                Play(GetParent());
            }
        };
    }

    private bool IsValidBody(Node body)
    {
        if (string.IsNullOrEmpty(body.Name))
        {
            return true;
        }

        return body.IsInGroup(RequiredGroup);
    }
}
