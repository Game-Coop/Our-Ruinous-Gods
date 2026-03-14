using Godot;
public interface IPower
{
    public PowerZone PowerZone { get; }
    public void Register(PowerZone powerZone);
}