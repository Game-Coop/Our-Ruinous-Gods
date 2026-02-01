using System;
using Godot;

[GlobalClass]
public partial class ItemData : GameResource
{
    [Export] public ItemCategory Category { get; set; }
    [Export] public int Count { get; set; }
    [Export] public Texture2D IconSprite { get; set; }
    [Export] public Texture2D PreviewSprite { get; set; }
    [Export] public Resource PreviewModel { get; set; }
  
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
