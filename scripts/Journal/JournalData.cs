using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(JournalData), "", nameof(Resource))]
public class JournalData : Resource
{
    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; }

    [Export(PropertyHint.MultilineText)]
    public string Content { get; set; }
    public JournalCategory Category { get; set; }
    public JournalData()
    {
        Id = 0;
        Name = "";
        Content = "";
        Category = JournalCategory.Logs;
    }
}