extends KinematicBody

var speed = 10
var gravity = 9.8
var jump = 7
var cameraAcceleration = 50
var sensitivity = 0.1
var snap
var direction = Vector3()
var velocity = Vector3()
var gravityDirection = Vector3()
var movement = Vector3();

onready var head = $Head;
onready var camera = $Head/Camera

func _ready() -> void:
	Input.set
func _input(event: InputEvent) -> void:
	
	if event is InputEventMouseMotion:
		camera.rotate_y(deg2rad(-event.relative.x * sensitivity))
		head.rotate_x(deg2rad(-event.relative.y * sensitivity))
		
		head.rotation.x = clamp(head.rotation.x, deg2rad(-90), deg2rad(90))
		
	pass
