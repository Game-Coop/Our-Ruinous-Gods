using System.Collections.Generic;
using System.Linq;
using Godot;

[GlobalClass]
public partial class PowerGrid : Resource
{
	[Export] public int MaxCharge = 10;

	private List<PowerZone> powerZones = new();
	
	public int CurrentCharge => powerZones.Sum(zone => zone.State == PowerState.On ? zone.Charge : 0);

	public void Register(PowerZone powerZone)
	{
		powerZones.Add(powerZone);
	}
}
