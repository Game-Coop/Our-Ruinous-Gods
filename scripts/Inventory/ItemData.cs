using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(ItemData), "", nameof(Resource))]
public class ItemData : Resource
{
    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; }
    [Export] public ItemCategory Category{ get; set; }
    [Export] public string Description{ get; set; }
    [Export] public int Count { get; set; }
    [Export] public Texture IconSprite { get; set; }
    [Export] public Texture PreviewSprite { get; set; }
    [Export] public Resource PreviewModel { get; set; }
    public bool IsCollected { get; set; }
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
