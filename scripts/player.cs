using Godot;
using System;
using System.Data.Common;
using System.IO.Compression;
public partial class Player : CharacterBody3D
{
	private Vector3 direction;
	[Export] private float gamepad_sensitivity = (float)1;
	private Node3D _head;
	private Node3D _camera;
	[Export] private float mouse_sensitivity = (float)0.001;
    [Export] public float gravity = 9.8f;  
	[Export] private float speed = 1;
	[Export] private float inertia = 1;
	private Vector3 velocity;
    private int Power = 0;
    private int MaxPower = 100;
    private int Stamina = 100;

    public override void _Ready()
    {;
        _head = GetNode<Node3D>("Head");
        _camera = GetNode<Node3D>("Head/Camera3D");

		Input.MouseMode = Input.MouseModeEnum.Captured;

        EventBus EventBusHandler = GetNode<EventBus>("/root/EventBus");
        EventBusHandler.Connect("PowerChangedEventHandler", new Callable(this, "OnPowerChange"));
        EventBusHandler.Connect("StaminaChangeEventHandler", new Callable(this, "OnStaminaChange"));
        EventBusHandler.Connect("WorldEventHandler", new Callable(this, "OnWorldEvent"));
    }
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion e)
		{
			_head.RotateY(-e.Relative.X * mouse_sensitivity);

			_camera.RotateX(-e.Relative.Y * mouse_sensitivity);
			_camera.Rotation = new Vector3(Mathf.Clamp(_camera.Rotation.X, Mathf.DegToRad(-80f), Mathf.DegToRad(90f)), _camera.Rotation.Y, _camera.Rotation.Z);
		}

		if (@event.IsActionPressed("quit")) GetTree().Quit();
	}
	public override void _PhysicsProcess(double delta) {
		//float turnHorizontal = (Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left")) * gamepad_sensitivity;
		//float turnVertical = (Input.GetActionStrength("look_down") - Input.GetActionStrength("look_up")) * gamepad_sensitivity;
		//HandelTurn(turnHorizontal, turnVertical);

		Vector3 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		Vector3 playerDirection = (_head.GlobalTransform.Basis * new Vector3(inputDirection.X, 0, inputDirection.Y)).Normalized();

		if (playerDirection != Vector3.Zero)
		{
			velocity.X = playerDirection.X * speed * (Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"));
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

	public void OnWorldEvent(string name) {
		GD.Print("event has occured: " + name);
	}

	public void OnStaminaChange(int cost) {
		this.Stamina -= cost;
		GD.Print("current stamina: " + this.Stamina);
	}
}