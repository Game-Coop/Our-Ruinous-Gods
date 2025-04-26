using Godot;
using System;

public class PowerCollisionTrigger : Spatial
{
	[Export] public int Zone { get; set; }
	[Export] public bool RunOnce { get; set; }
	private Area area;
	public override void _Ready()
	{
		area = GetNode<Area>("Area");
		area.Monitoring = true;
		area.Connect("body_entered", this, "OnBodyEntered");
	}
	public void OnBodyEntered(Node body)
	{
		if(body is player) {
			EventBus EventBusHandler = GetNode<EventBus>("/root/EventBus");      
			EventBusHandler.OnPowerEvent(this.Zone);

			if(RunOnce) {
				this.QueueFree();
			}
		}
	}
}
