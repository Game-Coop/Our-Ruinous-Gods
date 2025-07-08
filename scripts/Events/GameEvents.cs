
using System;

public static class GameEvents
{
    public static Action OnStartMenuLoad;
    public static Action<Player> OnRegisterPlayer;
    public static Action<Quest> OnQuestStart;
    public static Action<Quest> OnQuestComplete;
}