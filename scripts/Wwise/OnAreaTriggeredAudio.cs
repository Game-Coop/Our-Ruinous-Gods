using Godot;
using System;

public enum StateAudioTrigger
{
    Activated,
    Deactivated
}

public partial class OnAreaTriggeredAudio : AudioBehavior
{
    [Export] public StateAudioTrigger Trigger;
    [Export] public string RequiredGroup = "player";

    [Export] public NodePath RequiredPowerZonePath;
    private PowerZone requiredPowerZone;

    protected override void Setup()
    {
        if(RequiredPowerZonePath != null)
        {
            requiredPowerZone = GetNodeOrNull<PowerZone>(RequiredPowerZonePath);
        }

        var area = FindParentOfType<Area3D>();
        
        if (area == null) return;

        area.BodyEntered += body =>
        {
            if (!IsValidBody(body)) return;

            if (Trigger == StateAudioTrigger.Activated && CanActivate())
            {
                Play(GetParent());
            }
        };

        area.BodyExited += body =>
        {
            if (!IsValidBody(body)) return;

            if (Trigger == StateAudioTrigger.Deactivated && CanActivate())
            {
                Play(GetParent());
            }
        };
    }

    private bool CanActivate()
    {
        if (requiredPowerZone == null)
            return true;

        return requiredPowerZone.State == PowerState.On;
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
