using Godot;

public partial class ParleyTest : Node
{
    [Export] private Resource dialog;

    public override void _Ready()
    {
        base._Ready();
        ParleyFacts.SetValue("isAlive", false);
        CallDeferred(nameof(RunDialog));
    }
    public void RunDialog()
    {
        Parley.RunDialog(dialog);
    }
}