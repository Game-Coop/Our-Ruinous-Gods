using Godot;
using System;

public partial class PowerCollisionTrigger : Node3D, IPower
{
	public PowerZone PowerZone { get; private set; }
	[Export] public bool RunOnce { get; set; }
	private Area3D area;
	public override void _Ready()
	{
		area = GetNode<Area3D>("Area3D");
		area.Monitoring = true;
		area.Connect("body_entered", new Callable(this, "OnBodyEntered"));
	}
	public void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			PowerZone.TurnOff();
			if (RunOnce)
			{
				this.QueueFree();
			}
		}
	}
	public void Register(PowerZone powerZone)
	{
		PowerZone = powerZone;
	}
}
