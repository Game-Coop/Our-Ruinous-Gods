using Godot;
using System;

public partial class WwiseEventEmitter : Node
{
    [ExportCategory("Wwise")]
    [Export] public string BankName;
    [Export] public string EventName;

    [ExportCategory("Auto Trigger")]
    [Export] public bool PlayOnReady = false;
    [Export] public bool PlayOnInteract = false;
    [Export] public bool PlayOnEnable = false;

    public override void _Ready()
    {
        if(!string.IsNullOrEmpty(BankName))
        {
            WwiseManager.LoadBank(BankName);
        }

        if (PlayOnReady)
        {
            Play();
        }
    }

    public override void _Notification(int what)
    {
        if(what == NotificationEnabled && PlayOnEnable)
        {
            Play();
        }
    }

    public void Play()
    {
        if (string.IsNullOrEmpty(EventName))
        {
            return;
        }

        WwiseManager.PostEvent(EventName, this);
    }
}
