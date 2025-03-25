using Godot;
public interface IPower
{
    PowerState State { get; }
    int Charge { get; set; }
    int Zone { get; set; }
    void OnPowerChange(PowerEvent e);
}