
using Godot;
using Godot.Collections;

/// <summary>
/// This class is on autolaod so GdScripts can directly access to these functions
/// </summary>
public partial class ParleyFacts : Node
{
    private static Dictionary<string, Variant> parleyVariables = new();

    public override void _Ready()
    {
        base._Ready();
    }
    public static void SetValue(string key, Variant value)
    {
        if (parleyVariables.ContainsKey(key))
        {
            parleyVariables[key] = value;
        }
        else
        {
            parleyVariables.Add(key, value);
        }
    }
    public static Variant GetValue(string key)
    {
        parleyVariables.TryGetValue(key, out var value);
        return value;
    }
}