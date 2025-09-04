
using System;

public static class CharacterEvents
{
	//example
	public static Action<float> OnGainOxygen;
	public static Action<float> OnSpendOxygen;
	public static Action OnDie;
	public static Action OnRespawn;
	
	public static void RaiseGainOxygen(float v) => OnGainOxygen?.Invoke(v);
	public static void RaiseSpendOxygen(float v) => OnSpendOxygen?.Invoke(v);
	public static void RaiseDie() => OnDie?.Invoke();
	public static void RaiseRespawn() => OnRespawn?.Invoke();
}
