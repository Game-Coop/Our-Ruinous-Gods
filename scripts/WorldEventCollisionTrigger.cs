using Godot;
using System;

public partial class WorldEventCollisionTrigger : Node3D
{
	[Export] public string EventName { get; set; }
	private Area3D area;
	public override void _Ready()
	{
		area = GetNode<Area3D>("Area3D");
		area.Monitoring = true;
		area.Connect("body_entered", new Callable(this, "OnBodyEntered"));
		area.Connect("area_entered", new Callable(this, "OnBodyEntered"));
	}
	public void OnBodyEntered(Node body)
	{
		EventBus EventBusHandler = GetNode<EventBus>("/root/EventBus");      
        EventBusHandler.OnWorldEvent(this.EventName);
	}
}
