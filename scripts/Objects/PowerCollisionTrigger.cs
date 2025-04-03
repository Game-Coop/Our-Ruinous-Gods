using Godot;
using System;

public class PowerCollisionTrigger : Spatial
{
	[Export] public int Zone { get; set; }
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
        EventBusHandler.OnPowerEvent(this.Zone);
	}
}
