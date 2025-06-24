
using System.Collections.Generic;
using Godot;

[System.Serializable]
public partial class SaveData
{
    public GameState gameState;
    public bool playerDiedBefore; // TODO: we could specify this either with game state or a boolean. if dying will occur only once, its probably be better to use gamestate and have a specific gamestate for it.
    public PlayerData playerData;
    public CollectibleData collectibleData = new CollectibleData();
}

[System.Serializable]
public class CollectibleData
{
    public List<int> ItemIds = new List<int>();
    public List<int> JournalIds = new List<int>();
    public List<int> AudioIds = new List<int>();
}

[System.Serializable]
public class PlayerData
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 stamina;
}