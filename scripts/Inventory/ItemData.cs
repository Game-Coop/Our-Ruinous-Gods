using System;
using Godot;

[GlobalClass]
public partial class ItemData : Resource
{
    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; }
    [Export] public ItemCategory Category { get; set; }
    [Export] public string Description { get; set; }
    [Export] public int Count { get; set; }
    [Export] public Texture2D IconSprite { get; set; }
    [Export] public Texture2D PreviewSprite { get; set; }
    [Export] public Resource PreviewModel { get; set; }
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
                if(_isCollected)
                    OnCollected?.Invoke();
            }
        }
    }
    public ItemData()
    {
        Id = 0;
        Name = "";
        Category = ItemCategory.KeyItem;
        Description = "";
        Count = 1;
        IconSprite = null;
        PreviewSprite = null;
        PreviewModel = null;
    }
}
