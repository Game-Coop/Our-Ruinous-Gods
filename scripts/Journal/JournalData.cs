using Godot;

[GlobalClass]
public partial class JournalData : GameResource
{
    [Export(PropertyHint.MultilineText)] public string Content { get; set; }
    [Export] public JournalCategory Category { get; set; }
    public JournalData()
    {
        Id = 0;
        Name = "";
        Content = "";
        Category = JournalCategory.Log;
    }
}