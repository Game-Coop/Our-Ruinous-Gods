using Godot;
public interface IPower
{
    PowerState State { get; set; }
    int Charge { get; set; }
    int Zone { get; set; }
}