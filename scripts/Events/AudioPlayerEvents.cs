using System;

public static class AudioPlayerEvents
{
    public static Action OnAudioPlayerStoped;
    public static Action OnAudioPlayerFinished;
    public static Action<AudioData> OnAudioCollect;
}