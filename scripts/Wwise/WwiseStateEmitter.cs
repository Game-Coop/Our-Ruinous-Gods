using Godot;
using System;

public partial class WwiseStateEmitter : Node
{
    [ExportCategory("Wwise")]
    [Export] public string StateGroupName;

    [Export] public string StateName;

    public void Apply()
    {
        if(string.IsNullOrEmpty(StateGroupName) || string.IsNullOrEmpty(StateName))
        {
            return;
        }

        WwiseManager.SetState(StateGroupName, StateName);
    }
}
