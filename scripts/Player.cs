using Godot;
using System;

public struct CameraClamp
{
	public float Min { get; }
	public float Max { get; }

	public CameraClamp(float min, float max)
	{
		Min = min;
		Max = max;
	}

	public override string ToString() => $"({Min} - {Max})";
}

public partial class Player : CharacterBody3D, ISavable<SaveData>
{
	[Export] public float gravity = 9.8f;
	[Export] public float speed = 1.42f;

	private CameraClamp cameraClamp = new CameraClamp(-75f, 80f);

	private Node3D _head;
	private Node3D _camera;

	private int _ladderOverlapCount = 0;
	private bool _isOnLadder => _ladderOverlapCount > 0;
	private RayCast3D _rayDown;

	private int Power = 0;
	private int MaxPower = 100;
	private int Stamina = 100;
	private EventBus eventBus;
	public override void _Ready()
	{
		_head = GetNode<Node3D>("Head");
		_camera = GetNode<Node3D>("Head/Camera3D");
		_rayDown = GetNode<RayCast3D>("Head/Camera3D/RayCast3D");

		eventBus = GetNode<EventBus>("/root/EventBus");
		eventBus.PowerChanged += OnPowerChange;
		eventBus.StaminaChange += OnStaminaChange;
		eventBus.World += OnWorldEvent;

		GameEvents.OnRegisterPlayer.Invoke(this);

		if (SaveManager.SaveData != null && !GameManager.Instance.InStartMenu)
			OnLoad(SaveManager.SaveData);
	}
	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		eventBus.PowerChanged -= OnPowerChange;
		eventBus.StaminaChange -= OnStaminaChange;
		eventBus.World -= OnWorldEvent;
    }

	public override void _Input(InputEvent @event)
	{
		if (GameManager.Instance.InCutscene || GameManager.Instance.InStartMenu) return;
		if (@event is InputEventJoypadMotion || @event is InputEventMouseMotion)
		{
			if (@event is InputEventMouseMotion mouse)
			{
				_head.RotateY(-mouse.Relative.X * (float)0.001);

				_camera.RotateX(-mouse.Relative.Y * (float)0.001);
				_camera.Rotation = new Vector3(Mathf.Clamp(_camera.Rotation.X, Mathf.DegToRad(cameraClamp.Min), Mathf.DegToRad(cameraClamp.Max)), _camera.Rotation.Y, _camera.Rotation.Z);
			}

		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (_isOnLadder)
		{
			float climbInput = 0f;
			if (Input.IsActionPressed("move_forward"))
				climbInput = 1f; // Climb up
			else if (Input.IsActionPressed("move_backward"))
				climbInput = -1f; // Climb down

			if (climbInput < 0 && IsOnFloor() && _rayDown.IsColliding())
			{
				_ladderOverlapCount = 0;
			}
			else
			{
				velocity.X = 0;
				velocity.Z = 0;
				velocity.Y = climbInput * speed;
				Velocity = velocity;
				MoveAndSlide();
				return;
			}
		}

		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;
		else
			velocity.Y = 0;

		_head.RotateY(-(Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left")) * (float)0.02);

		_camera.RotateX(-(Input.GetActionStrength("look_down") - Input.GetActionStrength("look_up")) * (float)0.02);
		_camera.Rotation = new Vector3(Mathf.Clamp(_camera.Rotation.X, Mathf.DegToRad(cameraClamp.Min), Mathf.DegToRad(cameraClamp.Max)), _camera.Rotation.Y, _camera.Rotation.Z);

		Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		Vector3 playerDirection = (_head.GlobalTransform.Basis * new Vector3(inputDirection.X, 0, inputDirection.Y)).Normalized();

		if (playerDirection != Vector3.Zero)
		{
			velocity.X = playerDirection.X * (speed * 2);
			velocity.Z = playerDirection.Z * (speed * 2);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void OnPowerChange(PowerEvent e)
	{
		int currentPower = Power;

		if (e.State == PowerState.On)
		{
			currentPower += e.Charge;
		}
		else
		{
			currentPower -= e.Charge;
		}

		if (currentPower < 0) currentPower = 0;
		if (currentPower >= MaxPower) currentPower = MaxPower;

		Power = currentPower;
		GD.Print("current power use is: " + Power);
	}

	public void OnWorldEvent(string name)
	{
		GD.Print("event has occured: " + name);
	}

	public void OnStaminaChange(int cost)
	{
		this.Stamina -= cost;
		GD.Print("current stamina: " + this.Stamina);
	}

	public void OnSave(SaveData data)
	{
		GD.Print("on save player");
		if (data.playerData == null)
		{
			data.playerData = new PlayerData();
		}
		data.playerData.position = GlobalPosition;
		data.playerData.rotation = GlobalRotation;
		data.playerData.cameraRotation = _camera.Rotation;
		data.playerData.headRotation = _head.Rotation;
	}

	public void OnLoad(SaveData data)
	{
		if (data.playerData != null)
		{
			GD.Print("loadded player position");
			GlobalPosition = data.playerData.position;
			GlobalRotation = data.playerData.rotation;
			_camera.Rotation = data.playerData.cameraRotation;
			_head.Rotation = data.playerData.headRotation;
		}
	}
	private void OnAreaEntered()
	{
		_ladderOverlapCount++;
		GD.Print("Player entered ladder");
	}

	private void OnAreaExited()
	{
		_ladderOverlapCount--;
		if (_ladderOverlapCount < 0) _ladderOverlapCount = 0; // Prevent negative count
		GD.Print("Player exited ladder");
	}
}
