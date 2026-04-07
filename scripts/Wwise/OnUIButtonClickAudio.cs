using Godot;
using System;

public partial class OnUIButtonClickAudio : AudioBehavior
{
    protected override void Setup()
    {
        var button = FindParentOfType<BaseButton>();
        if(button == null)
        {
            GD.PushWarning($"{Name}: no basebutton found in parent chain");
            return;
        }

        button.Pressed += () => Play(button);
    }
}
