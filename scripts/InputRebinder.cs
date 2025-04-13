using Godot;
using System.Collections.Generic;
using System.Linq;

public static class InputRebinder
{
    /// <summary>
    /// Rebinds an input action to a new key.
    /// </summary>
    public static void RebindKey(string actionName, KeyList newKey)
    {
        if (!InputMap.HasAction(actionName))
        {
            GD.PrintErr($"Action '{actionName}' does not exist.");
            return;
        }

        RemoveEvents(actionName, typeof(InputEventKey)); // Remove existing keyboard bindings

        InputEventKey newKeyEvent = new InputEventKey
        {
            Scancode = (uint)newKey
        };

        InputMap.ActionAddEvent(actionName, newKeyEvent);
        GD.Print($"Rebound action '{actionName}' to key '{newKey}'.");
    }

    /// <summary>
    /// Rebinds an input action to a new gamepad button.
    /// </summary>
    public static void RebindGamepad(string actionName, int buttonIndex)
    {
        if (!InputMap.HasAction(actionName))
        {
            GD.PrintErr($"Action '{actionName}' does not exist.");
            return;
        }

        RemoveEvents(actionName, typeof(InputEventJoypadButton)); // Remove existing gamepad bindings

        InputEventJoypadButton newGamepadEvent = new InputEventJoypadButton
        {
            ButtonIndex = buttonIndex
        };

        InputMap.ActionAddEvent(actionName, newGamepadEvent);
        GD.Print($"Rebound action '{actionName}' to gamepad button '{buttonIndex}'.");
    }

    /// <summary>
    /// Removes specific types of input events (e.g., only gamepad or only keyboard events).
    /// </summary>
    public static void RemoveEvents(string actionName, System.Type eventType)
    {
        if (!InputMap.HasAction(actionName))
        {
            GD.PrintErr($"Action '{actionName}' does not exist.");
            return;
        }

        var eventsToRemove = new List<InputEvent>();

        foreach (InputEvent evt in InputMap.GetActionList(actionName))
        {
            if (eventType.IsInstanceOfType(evt))
            {
                eventsToRemove.Add(evt);
            }
        }

        foreach (var evt in eventsToRemove)
        {
            InputMap.ActionEraseEvent(actionName, evt);
        }

        GD.Print($"Removed {eventsToRemove.Count} events of type '{eventType.Name}' from action '{actionName}'.");
    }

    /// <summary>
    /// Swaps the bindings of two input actions.
    /// </summary>
    public static void SwapKeyboardBindings(string actionOne, string actionTwo)
    {
        SwapSpecificBindings<InputEventKey>(actionOne, actionTwo);
    }
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

        // Convert Godot Array to IEnumerable<InputEvent>, filter by type, and duplicate
        var actionOneEvents = InputMap.GetActionList(actionOne)
            .Cast<InputEvent>()
            .Where(e => e is T)
            .Select(e => e.Duplicate() as InputEvent)
            .ToList();

        var actionTwoEvents = InputMap.GetActionList(actionTwo)
            .Cast<InputEvent>()
            .Where(e => e is T)
            .Select(e => e.Duplicate() as InputEvent)
            .ToList();

        // Remove only the selected type of events
        InputMap.GetActionList(actionOne).Cast<InputEvent>().Where(e => e is T).ToList().ForEach(e => InputMap.ActionEraseEvent(actionOne, e));
        InputMap.GetActionList(actionTwo).Cast<InputEvent>().Where(e => e is T).ToList().ForEach(e => InputMap.ActionEraseEvent(actionTwo, e));

        // Swap bindings
        actionTwoEvents.ForEach(e => InputMap.ActionAddEvent(actionOne, e));
        actionOneEvents.ForEach(e => InputMap.ActionAddEvent(actionTwo, e));

        GD.Print($"Swapped {typeof(T).Name} bindings between '{actionOne}' and '{actionTwo}'.");
    }
}
