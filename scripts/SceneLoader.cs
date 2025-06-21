using Godot;
using System;
using System.Threading.Tasks;

public static partial class SceneLoader
{
    public static async Task LoadSceneAsync(this SceneTree sceneTree, PackedScene newPackedScene, Func<Node, Node, Task>? beforeUnload = null)
    {
        var newScene = newPackedScene.Instantiate();
        sceneTree.Root.AddChild(newScene);

        if (beforeUnload != null)
            await beforeUnload(sceneTree.CurrentScene, newScene);

        sceneTree.CurrentScene.QueueFree();
    }
    public static async Task LoadSceneAsync(this SceneTree sceneTree, Node newScene, Func<Node, Node, Task>? beforeUnload = null)
    {
        if (beforeUnload != null)
            await beforeUnload(sceneTree.CurrentScene, newScene);

        sceneTree.CurrentScene.QueueFree();
    }
}
