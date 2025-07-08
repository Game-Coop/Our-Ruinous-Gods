using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class Quest : Resource
{
    [Export] public Resource prefab { get; private set; }
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
                _instance = Questify.Instantiate(prefab);
            }
            return _instance;
        }
    }
    public override bool Equals(object obj)
    {
        if (obj is Resource resource)
        {
            return Questify.GetResourcePath(resource) == prefab.ResourcePath;
        }
        return false;
    }
    public void Start()
    {
        Questify.StartQuest(Instance);
        GameEvents.OnQuestStart?.Invoke(this);
    }
    public void Complete()
    {
        if(nextQuest != null)
            GD.Print($"Finished quest: {Name}. Will be starting quest: {(nextQuest != null ? nextQuest.Name : null)}");
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