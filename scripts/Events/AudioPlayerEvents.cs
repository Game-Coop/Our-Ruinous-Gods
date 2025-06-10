using System;
using System.Collections.Generic;

public static class AudioPlayerEvents
{
    public static Action OnAudioPlayerStoped;
    public static Action OnAudioPlayerFinished;
    public static Action<AudioData> OnAudioCollect;
    public static Action<Dictionary<int, AudioData>> OnAudioPlayerChange;
    public static Action OnUpdateRequest;
}