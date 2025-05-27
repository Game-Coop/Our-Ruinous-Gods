using Godot;
using System;
using System.IO.Compression;

public partial class Player : CharacterBody3D
{
	private Vector3 direction;
	[Export] private float gamepad_sensitivity = (float)1;
	private Node3D head;
	[Export] private float mouse_sensitivity = (float)1;
    [Export] private float gravity = 9.8f;  
	[Export] private float speed = 1;
	[Export] private float inertia = 1;
	private Vector3 velocity;
    private int Power = 0;
    private int MaxPower = 100;
    private int Stamina = 100;

    public override void _Ready()
    {;
        head = GetNode<Node3D>("Head");

        Input.MouseMode = Input.MouseModeEnum.Captured;

        EventBus EventBusHandler = GetNode<EventBus>("/root/EventBus");
        EventBusHandler.Connect("PowerChangedEventHandler", new Callable(this, "OnPowerChange"));
        EventBusHandler.Connect("StaminaChangeEventHandler", new Callable(this, "OnStaminaChange"));
        EventBusHandler.Connect("WorldEventHandler", new Callable(this, "OnWorldEvent"));
    }
	
	public override void _Input(InputEvent e)
	{
		if (e is InputEventMouseMotion) handleMouseLook(e as InputEventMouseMotion);
		// if (e.IsActionPressed("quit")) GetTree().Quit();
	}

	public void OnPowerChange(PowerEvent e) {
		int currentPower = Power;

		if(e.State == PowerState.On) {
			currentPower += e.Charge;
		} else {
			currentPower -= e.Charge;
		}

		if(currentPower < 0) currentPower = 0;
		if(currentPower >= MaxPower) currentPower = MaxPower;

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

	public override void _PhysicsProcess(double delta) {
		float turnHorizontal = (Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left")) * gamepad_sensitivity;
		float turnVertical = (Input.GetActionStrength("look_down") - Input.GetActionStrength("look_up")) * gamepad_sensitivity;

		HandelMove(delta);
		HandelTurn(turnHorizontal, turnVertical);
	}

	private void HandelMove(double delta) {
		float horizontalRotion = GlobalTransform.Basis.GetEuler().Y;
		float movement = Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward");
		float strafe = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");

		Vector3 playerVelocity = Velocity;

		direction = Vector3.Zero;
		direction = new Vector3(strafe, 0, movement);
		direction = direction.Rotated(Vector3.Up, horizontalRotion).Normalized();
		
        playerVelocity += Vector3.Down * gravity * (float)delta;
		playerVelocity = velocity.Lerp(direction * speed, (float)delta / inertia);

		Velocity = playerVelocity;


		MoveAndSlide();
	}

	private void HandelTurn(float x, float y) {
		float clampDegrees = 90;

		RotateY(Mathf.DegToRad(-x));
		
		head.RotateX(Mathf.DegToRad(-y));
		head.Rotation = new Vector3(Mathf.Clamp(head.Rotation.X, Mathf.DegToRad(-clampDegrees), Mathf.DegToRad(clampDegrees)), head.Rotation.Y, head.Rotation.Z);
	}

	private void handleMouseLook(InputEventMouseMotion e) {
		HandelTurn(e.Relative.X * mouse_sensitivity, e.Relative.Y * mouse_sensitivity);
	}
}