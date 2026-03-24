using Godot;
using System;

public partial class OnInteractAudio : AudioBehavior
{
    protected override void Setup()
    {
        var interactable = FindParentOfType<IInteractable>();
        if (interactable != null)
        {
            interactable.OnInteract += _ => Play(GetParent());
        }
    }
}
