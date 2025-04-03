using Godot;
using System;

public class WorldEventCollisionTrigger : Spatial
{
	[Export] public string EventName { get; set; }
	private Area area;
	public override void _Ready()
	{
		area = GetNode<Area>("Area");
		area.Monitoring = true;
		area.Connect("body_entered", this, "OnBodyEntered");
		area.Connect("area_entered", this, "OnBodyEntered");
	}
	public void OnBodyEntered(Node body)
	{
		EventBus EventBusHandler = GetNode<EventBus>("/root/EventBus");      
        EventBusHandler.OnWorldEvent(this.EventName);
	}
}
