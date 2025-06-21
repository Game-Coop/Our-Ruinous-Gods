
using System.Threading.Tasks;
using Godot;

public partial class SceneTransitioner : Path3D
{
	[Export] private float _duration = 10f;
	[Export] private PathFollow3D pathFollow3D;
	[Export] private Tween.TransitionType transitionType = Tween.TransitionType.Sine;
	[Export] private Tween.EaseType easeType = Tween.EaseType.InOut;
	public async void TransitionTo(Node3D newScene, Node3D objectToMove, float duration = -1)
	{
		if (duration != -1) _duration = duration;

		await GetTree().LoadSceneAsync(newScene, async (_, newScene) =>
		{
			await ObjectPathTween(objectToMove);
		});
	}
	public async void TransitionTo(PackedScene newScene, Node3D objectToMove, float duration = -1)
	{
		if (duration != -1) _duration = duration;

		await GetTree().LoadSceneAsync(newScene, async (_, _) =>
		{
			await ObjectPathTween(objectToMove);
		});
	}

	private async Task ObjectPathTween(Node3D objectToMove)
	{
		var tween = CreateTween();
		objectToMove.GetParent().RemoveChild(objectToMove);
		pathFollow3D.AddChild(objectToMove);
		objectToMove.Position = Vector3.Zero;
		objectToMove.Rotation = Vector3.Zero;

		tween.TweenProperty(pathFollow3D, "progress_ratio", 1.0f, _duration)
		.SetTrans(Tween.TransitionType.Sine)
		.SetEase(Tween.EaseType.InOut);
		await ToSignal(tween, "finished");
	}

}
