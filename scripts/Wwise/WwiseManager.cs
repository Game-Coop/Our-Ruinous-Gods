using Godot;
using System;

public static class WwiseManager
{
    private static Node _wwise;
    private static Node Wwise
    {
        get
        {
            if (_wwise == null && Engine.GetMainLoop() is SceneTree tree)
            {
                _wwise = tree.Root.GetNodeOrNull("WwiseBridge");
            }
            return _wwise;
        }
    }
    public static bool LoadBank(uint bankId)
    {
        GD.Print("loading bank");
        return (bool)CallWwise("load_bank_id", bankId);
    }
    public static bool UnloadBank(uint bankId)
    {
        return (bool)CallWwise("unload_bank_id", bankId);
    }
    public static void PostEvent(uint eventId, Node gameObject)
    {
        GD.Print($"[Wwise] Posting event: {eventId} on {gameObject.Name}");
        CallWwise("post_event_id", eventId, gameObject);
    }
    public static uint PostEventCallback(uint eventId, uint callback_mask, Node game_object, Callable callback)
    {
        return (uint)CallWwise("post_event_id_callback", eventId, callback_mask, game_object, callback);
    }
    public static void StopEvent(uint eventId, Node gameObject)
    {
        CallWwise("stop_event_id", eventId, gameObject);
    }
    public static bool SetRTPCValue(uint rtpcId, float value)
    {
        return (bool)CallWwise("set_rtpc_value_id", rtpcId, value);
    }
    public static float GetRTPCValue(uint rtpcId)
    {
        return (float)CallWwise("get_rtpc_value_id", rtpcId);
    }
    public static void SetSwitch(uint switchGroupId, uint switchStateId, Node gameObject)
    {
        CallWwise("set_switch_id", switchGroupId, switchStateId, gameObject);
    }
    public static void SetState(uint stateGroupId, uint state)
    {
        CallWwise("set_state_id", stateGroupId, state);
    }
    public static bool RegisterGameObject(string gameObjectName)
    {
        return (bool)CallWwise("register_game_obj", gameObjectName);
    }
    public static bool UnregisterGameObject(string gameObjectName)
    {
        return (bool)CallWwise("unregister_game_obj", gameObjectName);
    }
    public static bool Set3dPosition(string gameObjectName, Transform3D transform)
    {
        return (bool)CallWwise("set_3d_position", gameObjectName, transform);
    }
    private static Variant CallWwise(string methodName, params Variant[] args)
    {
        if (Wwise == null)
        {
            GD.PrintErr("wwise was null");
            return default;
        }
        return Wwise.Call(methodName, args);
    }
}
