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
        return (bool)CallWwise("load_bank_id", bankId);
    }
    public static bool LoadBank(string bankName)
    {
        return (bool)CallWwise("load_bank", bankName);
    }
    public static bool UnloadBank(uint bankId)
    {
        return (bool)CallWwise("unload_bank_id", bankId);
    }
    public static bool UnloadBank(string bankName)
    {
        return (bool)CallWwise("unload_bank", bankName);
    }
    public static void PostEvent(uint eventId, Node gameObject)
    {
        GD.Print($"[Wwise] Posting event: {eventId} on {gameObject.Name}");
        CallWwise("post_event_id", eventId, gameObject);
    }
    public static void PostEvent(string eventName, Node gameObject)
    {
        GD.Print($"[Wwise] Posting event: {eventName} on {gameObject.Name}");
        CallWwise("post_event", eventName, gameObject);
    }
    public static uint PostEventCallback(uint eventId, uint callback_mask, Node game_object, Callable callback)
    {
        return (uint)CallWwise("post_event_id_callback", eventId, callback_mask, game_object, callback);
    }
    public static uint PostEventCallback(string eventName, uint callback_mask, Node game_object, Callable callback)
    {
        return (uint)CallWwise("post_event__callback", eventName, callback_mask, game_object, callback);
    }
    public static void StopEvent(uint eventId, Node gameObject)
    {
        CallWwise("stop_event_id", eventId, gameObject);
    }
    public static bool SetRTPCValue(uint rtpcId, float value, Node gameObject)
    {
        return (bool)CallWwise("set_rtpc_value_id", rtpcId, value, gameObject);
    }
    public static bool SetRTPCValue(string rtpcName, float value, Node gameObject)
    {
        return (bool)CallWwise("set_rtpc_value", rtpcName, value, gameObject);
    }
    public static float GetRTPCValue(uint rtpcId, Node gameObject)
    {
        return (float)CallWwise("get_rtpc_value_id", rtpcId, gameObject);
    }
    public static float GetRTPCValue(string rtpcName, Node gameObject)
    {
        return (float)CallWwise("get_rtpc_value", rtpcName, gameObject);
    }
    public static void SetSwitch(uint switchGroupId, uint switchStateId, Node gameObject)
    {
        CallWwise("set_switch_id", switchGroupId, switchStateId, gameObject);
    }
    public static void SetSwitch(string switchGroupName, string switchStateName, Node gameObject)
    {
        CallWwise("set_switch", switchGroupName, switchStateName, gameObject);
    }
    public static void SetState(uint stateGroupId, uint stateId)
    {
        CallWwise("set_state_id", stateGroupId, stateId);
    }
    public static void SetState(string stateGroupName, string stateName)
    {
        CallWwise("set_state", stateGroupName, stateName);
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
