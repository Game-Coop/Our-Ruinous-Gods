
using System;
using Godot;

public class PushButton : Area, IInteractable
{
    public event Action<BaseEventData> OnInteract;
    [Export] private NodePath switchableObjectPath;
    private ISwitchable switchable;

    public override void _Ready()
    {
        base._Ready();
        switchable = GetNode<ISwitchable>(switchableObjectPath);
    }
    public void Interact()
    {
        switchable.TurnOn();
        OnInteract?.Invoke(null);
    }

    //Temporary code. This will definetly be different
    public void OnBodyEntered(Node node)
    {
        GD.Print("Entered");
        // if (node.Name == "player")
        {
            Interact();
        }
    }
}