
using System;

public static class CharacterEvents
{
    //example
    public static Action<float> OnGainOxygen;
    public static Action<float> OnSpendOxygen;
    public static Action OnDie;
    public static Action OnRespawn;
}