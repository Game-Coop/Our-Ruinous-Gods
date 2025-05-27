using Godot;

[GlobalClass]
public partial class JournalData : Resource
{
    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; }
    [Export(PropertyHint.MultilineText)] public string Content { get; set; }
    [Export] public JournalCategory Category { get; set; }
    public bool IsCollected { get; set; }
    public JournalData()
    {
        Id = 0;
        Name = "";
        Content = "";
        Category = JournalCategory.Logs;
    }
}