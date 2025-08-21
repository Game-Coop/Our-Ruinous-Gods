
using Godot;
using Godot.Collections;

/// <summary>
/// Here we will add c# representatives of parley actions. This class is on autolaod 
/// so GdScripts can directly access to these functions
/// </summary>
public partial class ParleyActions : Node
{
    ///Example:
    public static void ChangeMode(Array args)
    {
        GD.Print(args[0]);
    }
}