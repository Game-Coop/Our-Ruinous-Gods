using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class Quest : Resource
{
	[Export] public Resource @ref { get; private set; }
	[Export] public Quest nextQuest { get; private set; }
	public List<QuestObjective> objectives = new();
	public string Name => (string)Instance.Get("name");
	public string Description => (string)Instance.Get("description");
	public bool Started => (bool)Instance.Get("started");
	public bool Completed => (bool)Instance.Get("completed");
	private Resource _instance;
	public Resource Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Questify.Instantiate(@ref);
			}
			return _instance;
		}
	}
	public void DisposeInstance()
	{
		_instance = null;
	}
	public override bool Equals(object obj)
	{
		if (obj is Resource resource)
		{
			return Questify.GetResourcePath(resource) == @ref.ResourcePath;
		}
		return false;
	}
	public void Start()
	{
		GD.Print($"Starting quest: {Name}. Description: {Description}. Completed: {Completed}");
		Questify.StartQuest(Instance);
		GameEvents.OnQuestStart?.Invoke(this);
	}
	public void Complete()
	{
		GD.Print($"Finished quest: {Name}. Description: {Description}");
		GameEvents.OnQuestComplete?.Invoke(this);
	}
}

public class QuestObjective
{
	public QuestObjective(Resource objective)
	{
		this.objective = objective;
	}
	private Resource objective;
	public string Description => (string)objective.Get("description");
	public bool HasNotified => (bool)objective.Get("_has_notified");
}
