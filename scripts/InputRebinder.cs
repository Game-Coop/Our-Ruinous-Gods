using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class InputRebinder
{
    /// <summary>
    /// Rebinds an input action to a new key.
    /// </summary>
    public static void RebindKey(string actionName, Key newKey)
    {
        if (!InputMap.HasAction(actionName))
        {
            GD.PrintErr($"Action '{actionName}' does not exist.");
            return;
        }

        RemoveEvents(actionName, typeof(InputEventKey)); // Remove existing keyboard bindings

        var newKeyEvent = new InputEventKey
        {
            Keycode = newKey
        };

        InputMap.ActionAddEvent(actionName, newKeyEvent);
        GD.Print($"Rebound action '{actionName}' to key '{newKey}'.");
    }

    /// <summary>
    /// Rebinds an input action to a new gamepad button.
    /// </summary>
    public static void RebindGamepad(string actionName, JoyButton buttonIndex)
    {
        if (!InputMap.HasAction(actionName))
        {
            GD.PrintErr($"Action '{actionName}' does not exist.");
            return;
        }

        RemoveEvents(actionName, typeof(InputEventJoypadButton)); // Remove existing gamepad bindings

        var newGamepadEvent = new InputEventJoypadButton
        {
            ButtonIndex = buttonIndex
        };

        InputMap.ActionAddEvent(actionName, newGamepadEvent);
        GD.Print($"Rebound action '{actionName}' to gamepad button '{buttonIndex}'.");
    }

    /// <summary>
    /// Removes specific types of input events (e.g., only gamepad or only keyboard events).
    /// </summary>
    public static void RemoveEvents(string actionName, Type eventType)
    {
        if (!InputMap.HasAction(actionName))
        {
            GD.PrintErr($"Action '{actionName}' does not exist.");
            return;
        }

        var events = InputMap.ActionGetEvents(actionName).OfType<InputEvent>().ToList();
        var toRemove = events.Where(e => eventType.IsInstanceOfType(e)).ToList();

        foreach (var evt in toRemove)
        {
            InputMap.ActionEraseEvent(actionName, evt);
        }

        GD.Print($"Removed {toRemove.Count} events of type '{eventType.Name}' from action '{actionName}'.");
    }

    /// <summary>
    /// Swaps the bindings of two input actions (keyboard only).
    /// </summary>
    public static void SwapKeyboardBindings(string actionOne, string actionTwo)
    {
        SwapSpecificBindings<InputEventKey>(actionOne, actionTwo);
    }

    /// <summary>
    /// Swaps the bindings of two input actions (gamepad only).
    /// </summary>
    public static void SwapGamepadBindings(string actionOne, string actionTwo)
    {
        SwapSpecificBindings<InputEventJoypadButton>(actionOne, actionTwo);
    }

    private static void SwapSpecificBindings<T>(string actionOne, string actionTwo) where T : InputEvent
    {
        if (!InputMap.HasAction(actionOne) || !InputMap.HasAction(actionTwo))
        {
            GD.PrintErr("One or both actions do not exist.");
            return;
        }

        var eventsA = InputMap.ActionGetEvents(actionOne).OfType<T>().Select(e => (InputEvent)e.Duplicate()).ToList();
        var eventsB = InputMap.ActionGetEvents(actionTwo).OfType<T>().Select(e => (InputEvent)e.Duplicate()).ToList();

        foreach (var e in InputMap.ActionGetEvents(actionOne).OfType<T>())
            InputMap.ActionEraseEvent(actionOne, e);

        foreach (var e in InputMap.ActionGetEvents(actionTwo).OfType<T>())
            InputMap.ActionEraseEvent(actionTwo, e);

        foreach (var e in eventsB)
            InputMap.ActionAddEvent(actionOne, e);

        foreach (var e in eventsA)
            InputMap.ActionAddEvent(actionTwo, e);

        GD.Print($"Swapped {typeof(T).Name} bindings between '{actionOne}' and '{actionTwo}'.");
    }
}
