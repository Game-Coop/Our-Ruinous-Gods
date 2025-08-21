using Godot;
using Godot.Collections;
using System;
public static class Parley
{
    #region Parley Instance
    private static readonly NodePath nodePath = new("/root/Parley");
    private static Node instance;
    public static Node Instance
    {
        get
        {
            if (instance == null)
            {
                instance = ((SceneTree)Engine.GetMainLoop()).Root.GetNode(nodePath);
            }
            return instance;
        }
    }
    private static Script _parleyContext;
    private static Script ParleyContext
    {
        get
        {
            if (_parleyContext == null)
                _parleyContext = (Script)GD.Load("res://addons/parley/models/parley_context.gd");

            return _parleyContext;
        }
    }
    #endregion

    public static Node RunDialog(Resource dialog)
    {
        var context = ParleyContext.Call("create", dialog);
        return Instance.Call("run_dialogue", context, dialog).AsGodotObject() as Node;
    }

}