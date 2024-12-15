using System;

public interface IInteractable
{
    event Action<BaseEventData> OnInteract;
    void Interact();
}

