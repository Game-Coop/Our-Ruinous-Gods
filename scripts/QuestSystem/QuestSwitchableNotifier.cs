
using System;
using Godot;

public partial class QuestSwitchableNotifier : Node
{
    [Export] private string questVariable;
    [Export] public NodePath switchableNode;
    private ISwitchable switchable;
    private EventBus eventBus;
    public override void _Ready()
    {
        base._Ready();
        eventBus = GetNode<EventBus>("/root/EventBus");
        switchable = GetNode<ISwitchable>(switchableNode);
        if (switchable != null)
            switchable.OnStateChange += OnStateChange;
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (switchable != null)
            switchable.OnStateChange -= OnStateChange;
    }
    private void OnStateChange(bool state)
    {
        eventBus.OnQuestEvent(questVariable, state);
    }
}