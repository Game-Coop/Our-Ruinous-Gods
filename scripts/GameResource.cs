using System;
using Godot;

public partial class GameResource : Resource
{
    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; }
    [Export] public string Description { get; set; }
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
}