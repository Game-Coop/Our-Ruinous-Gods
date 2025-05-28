using Godot;
using System;
using System.Data.Common;
using System.IO.Compression;
public partial class Player : CharacterBody3D
{
	[Export] public float gravity = 9.8f;
	[Export] private float speed = 1.42f;
	private Node3D _head;
	private Node3D _camera;
	private int Power = 0;
	private int MaxPower = 100;
	private int Stamina = 100;

	public override void _Ready()
	{
		;
		_head = GetNode<Node3D>("Head");
		_camera = GetNode<Node3D>("Head/Camera3D");

		Input.MouseMode = Input.MouseModeEnum.Captured;

		EventBus EventBusHandler = GetNode<EventBus>("/root/EventBus");
		EventBusHandler.PowerChanged += OnPowerChange;
		EventBusHandler.StaminaChange += OnStaminaChange;
		EventBusHandler.World += OnWorldEvent;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventJoypadMotion || @event is InputEventMouseMotion)
		{
			Vector2 look_direction = Vector2.Zero;

			if (@event is InputEventJoypadMotion)
			{
				look_direction.X = (Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left")) * (float)0.01;
				look_direction.Y = (Input.GetActionStrength("look_down") - Input.GetActionStrength("look_up")) * (float)0.01;
			}

			if (@event is InputEventMouseMotion mouse)
			{
				look_direction.X = mouse.Relative.X * (float)0.001;
				look_direction.Y = mouse.Relative.Y * (float)0.001;
			}

			_head.RotateY(-look_direction.X);

			_camera.RotateX(-look_direction.Y);
			_camera.Rotation = new Vector3(Mathf.Clamp(_camera.Rotation.X, Mathf.DegToRad(-75f), Mathf.DegToRad(80f)), _camera.Rotation.Y, _camera.Rotation.Z);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		Vector3 playerDirection = (_head.GlobalTransform.Basis * new Vector3(inputDirection.X, 0, inputDirection.Y)).Normalized();

		if (playerDirection != Vector3.Zero)
		{
			velocity.X = playerDirection.X * speed;
			velocity.Z = playerDirection.Z * speed;
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
}
