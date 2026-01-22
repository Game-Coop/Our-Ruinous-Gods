using Godot;
using System;

public partial class WwiseRtpcEmitter : Node
{
    [ExportCategory("Wwise")]
    [Export] public string RtcpName;

    [Export] public float MinValue = 0f;
    [Export] public float MaxValue = 1f;

    public void SetNormalized(float t)
    {
        if(string.IsNullOrEmpty(RtcpName))
        {
            return;
        }

        float value = Mathf.Lerp(MinValue, MaxValue, t);
        WwiseManager.SetRTPCValue(RtcpName, value, this);
    }
}
