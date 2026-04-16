using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public partial class SaveData
{
    public GameState gameState;
    public bool playerDiedBefore; // TODO: we could specify this either with game state or a boolean. if dying will occur only once, its probably be better to use gamestate and have a specific gamestate for it.
    public PlayerData playerData;
    public CollectibleData collectibleData = new CollectibleData();

    [JsonConverter(typeof(GodotCollectionsArrayConverter))]
    public Godot.Collections.Array questData = new Godot.Collections.Array();
    public List<int> solvedPuzzleIds = new List<int>();
    public Dictionary<int, PowerState> powerZoneStates = new();
}