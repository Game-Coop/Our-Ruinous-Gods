using Godot;
using System;

public interface IInteractable
{
    event Action<BaseEventData> OnInteract;
    string InteractionText { get; }
    bool CanInteract();
    void Interact();
    void ShowHint();
    void HideHint();
}

