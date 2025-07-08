
using System;
using System.Collections.Generic;
using Godot;

public partial class QuestsManager : Node
{
	[Export] public Quest activeQuest;
	private Dictionary<string, Variant> questVariables = new();
	private EventBus eventBus;
	public override void _Ready()
	{
		base._Ready();

		Questify.ConnectQuestCompleted(QuestComplete);
		Questify.ConnectConditionQueryRequested(ConditionQuerry);
		Questify.ConnectQuestObjectiveAdded(ObjectiveAdded);

		eventBus = GetNode<EventBus>("/root/EventBus");

		eventBus.PowerChanged += OnPowerChange;
		eventBus.World += OnWorldEvent;
		eventBus.Quest += OnQuestEvent;

		activeQuest.Start();
	}

	private void ObjectiveAdded(Resource questResource, Resource objective)
	{
		if (activeQuest.Equals(questResource))
		{
			activeQuest.objectives.Add(new QuestObjective(objective));
		}
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		eventBus.PowerChanged -= OnPowerChange;
		eventBus.World -= OnWorldEvent;
		eventBus.Quest -= OnQuestEvent;
	}
	private void OnQuestEvent(string questVariable, Variant value)
	{
		SetValue(questVariable, value);
		Questify.UpdateQuests();
	}

	private void OnWorldEvent(string eventName)
	{
		//Acts like a trigger
		SetValue(eventName, true);
		Questify.UpdateQuests();
		SetValue(eventName, false);
		Questify.UpdateQuests();
	}
	private void OnPowerChange(PowerEvent e)
	{
		var powerZoneStateVariable = $"power_zone_state_{e.Zone}";
		var powerZoneChargeVariable = $"power_zone_charge_{e.Zone}";
		SetValue(powerZoneStateVariable, e.State == PowerState.On);
		SetValue(powerZoneChargeVariable, e.Charge);
		Questify.UpdateQuests();
	}
	private Variant GetValue(string key)
	{
		questVariables.TryGetValue(key, out var value);
		return value;
	}
	private void SetValue(string eventName, Variant value)
	{
		if (questVariables.ContainsKey(eventName) == false)
		{
			questVariables.Add(eventName, value);
		}
		else
		{
			questVariables[eventName] = value;
		}
	}
	private void QuestComplete(Resource questResource)
	{
		if (activeQuest.Equals(questResource))
		{
			activeQuest.Complete();
			activeQuest = activeQuest.nextQuest;
			activeQuest?.Start();
		}
	}
	private void ConditionQuerry(string type, string key, Variant value, Resource resource)
	{
		var variable = GetValue(key);
		if (variable.VariantType == Variant.Type.Nil)
		{
			return;
		}
		bool result = false;
		switch (type)
		{
			case "eq":
			case "==":
				result = variable.Equals(value);
				break;
			case "neq":
			case "ne":
			case "!eq":
			case "!=":
				result = !variable.Equals(value);
				break;
			case "lt":
			case "<":
				if (variable.VariantType != Variant.Type.Float)
					throw new InvalidOperationException("Incorrect variable type for quest condition query operator");
				result = (float)variable < (float)value;
				break;
			case "lte":
			case "<=":
				if (variable.VariantType != Variant.Type.Float)
					throw new InvalidOperationException("Incorrect variable type for quest condition query operator");
				result = (float)variable <= (float)value;
				break;
			case "gt":
			case ">":
				if (variable.VariantType != Variant.Type.Float)
					throw new InvalidOperationException("Incorrect variable type for quest condition query operator");
				result = (float)variable > (float)value;
				GD.Print("should enter here");
				break;
			case "gte":
			case ">=":
				if (variable.VariantType != Variant.Type.Float)
					throw new InvalidOperationException("Incorrect variable type for quest condition query operator");
				result = (float)variable >= (float)value;
				break;
			default:
				GD.PrintErr($"Unknown operator '{type}' in quest condition query");
				break;
		}
		Questify.SetConditionCompleted(resource, result);
	}
}
