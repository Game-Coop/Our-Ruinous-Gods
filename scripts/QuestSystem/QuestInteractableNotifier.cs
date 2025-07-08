
using System;
using Godot;

public partial class QuestInteractableNotifier : Node
{
    [Export] private string questVariable;
    [Export] public NodePath interactableNode;
    private IInteractable interactable;
    private EventBus eventBus;
    public override void _Ready()
    {
        base._Ready();
		eventBus = GetNode<EventBus>("/root/EventBus");
        interactable = GetNode<IInteractable>(interactableNode);
        if (interactable != null)
            interactable.OnInteract += OnInteract;
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (interactable != null)
            interactable.OnInteract -= OnInteract;
    }
    private void OnInteract(BaseEventData data)
    {
        eventBus.OnQuestEvent(new QuestEvent(questVariable, true));
    }
}