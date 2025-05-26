using Godot;

[RegisteredType(nameof(ItemData), "", nameof(Resource))]
public partial class ItemData : Resource
{
    [Export] public int Id { get; set; }
    [Export] public string Name { get; set; }
    [Export] public ItemCategory Category{ get; set; }
    [Export] public string Description{ get; set; }
    [Export] public int Count { get; set; }
    [Export] public Texture2D IconSprite { get; set; }
    [Export] public Texture2D PreviewSprite { get; set; }
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
