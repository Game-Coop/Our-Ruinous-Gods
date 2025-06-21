using Godot;
using System;
using System.Threading.Tasks;

public static class SceneLoader
{
    /// <summary>
    /// Loads scene from given node then unloads the previous scene
    /// </summary>
    /// <param name="sceneTree">Sceen Tree</param>
    /// <param name="newScene">New scene to load</param>
    public static void LoadScene(this SceneTree sceneTree, Node newScene)
    {
        sceneTree.CurrentScene.QueueFree();
        sceneTree.CurrentScene = newScene;
    }
    /// <summary>
    /// LoadScene from given packedScene
    /// </summary>
    /// <param name="sceneTree">Sceen Tree</param>
    /// <param name="newPackedScene">New scene to load</param>
    /// <param name="additive">If set to true, it will not unload the previous scene</param>
    /// <param name="defer">If set to true, it will defer to call add child</param>
    /// <returns></returns>
    public static Node LoadScene(this SceneTree sceneTree, PackedScene newPackedScene, bool additive = false, bool defer = false)
    {
        var newScene = newPackedScene.Instantiate();
        if (defer)
            sceneTree.Root.CallDeferred("add_child", newScene);
        else
            sceneTree.Root.AddChild(newScene);

        if (!additive)
        {
            sceneTree.CurrentScene.QueueFree();
            sceneTree.CurrentScene = newScene;
        }
        return newScene;
    }
    /// <summary>
    /// Loads scene from given PackedScene asynchronously. Before unloading the previous scene it will wait for the given beforeUnload task to finish.
    /// </summary>
    /// <param name="sceneTree">Sceen Tree</param>
    /// <param name="newPackedScene">New scene to load</param>
    /// <param name="beforeUnload">Task that will be finished before unload</param>
    /// <returns></returns>
    public static async Task LoadSceneAsync(this SceneTree sceneTree, PackedScene newPackedScene, Func<Node, Node, Task>? beforeUnload = null)
    {
        var newScene = newPackedScene.Instantiate();
        sceneTree.Root.AddChild(newScene);

        if (beforeUnload != null)
            await beforeUnload(sceneTree.CurrentScene, newScene);

        sceneTree.CurrentScene.QueueFree();
        sceneTree.CurrentScene = newScene;
    }
    /// <summary>
    /// Loads scene from given node asynchronously. Before unloading the previous scene it will wait for the given beforeUnload task to finish.
    /// </summary>
    /// <param name="sceneTree">Sceen Tree</param>
    /// <param name="newScene">New scene to load</param>
    /// <param name="beforeUnload">Task that will be finished before unload</param>
    /// <returns></returns>
    public static async Task LoadSceneAsync(this SceneTree sceneTree, Node newScene, Func<Node, Node, Task>? beforeUnload = null)
    {
        if (beforeUnload != null)
            await beforeUnload(sceneTree.CurrentScene, newScene);

        sceneTree.CurrentScene.QueueFree();
        sceneTree.CurrentScene = newScene;
    }
}
