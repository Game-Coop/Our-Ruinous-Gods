using Godot;
using System;

public interface IInteractable
{
    event Action<BaseEventData> OnInteract;
    string InteractionText { get; }
    void Interact();
    void ShowHint();
    void HideHint();
}

