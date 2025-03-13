using Godot;
public interface IWorldEvent
{
    string name { get; set; } 
    void OnWorldEvent(WorldEvent e);
}