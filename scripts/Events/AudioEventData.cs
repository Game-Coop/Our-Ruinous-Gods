using Godot;
using System;

[GlobalClass]
public partial class AudioEventData : Resource
{
    [Export] public string BankName;
    [Export] public string EventName;
    [Export] public bool LoadBankOnReady = true;
}
