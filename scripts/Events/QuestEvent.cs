using Godot;

public partial class QuestEvent : GodotObject 
{
    public string variableName;
    public Variant variable;

    public QuestEvent(string variableName, Variant variable)
    {
        this.variableName = variableName;
        this.variable = variable;
    }
}