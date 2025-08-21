using System;
using Godot;

[GlobalClass]
public partial class JournalData : Resource
{
    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; }
    [Export(PropertyHint.MultilineText)] public string Content { get; set; }
    [Export] public JournalCategory Category { get; set; }
    public event Action OnCollected;
    private bool _isCollected;
    public bool IsCollected
    {
        get { return _isCollected; }
        set
        {
            if (value != _isCollected)
            {
                _isCollected = value;
                if (_isCollected)
                    OnCollected?.Invoke();
            }
        }
    }
    public JournalData()
    {
        Id = 0;
        Name = "";
        Content = "";
        Category = JournalCategory.Logs;
    }
}