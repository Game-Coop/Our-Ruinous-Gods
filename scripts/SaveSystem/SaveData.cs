
using System.Collections.Generic;
using System.Numerics;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;
    public Vector3 playerRot;
    public GameState gameState;
    public bool playerDiedBefore; // TODO: we could specify this either with game state or a boolean. if dying will occur only once, its probably be better to use gamestate and have a specific gamestate for it.
    public CollectibleData collectibleData = new CollectibleData();
}

[System.Serializable]
public class CollectibleData
{
    public List<int> ItemIds = new List<int>();
    public List<int> JournalIds = new List<int>();
    public List<int> AudioIds = new List<int>();
}