class_name ShieldBullet

extends Node2D

var direction: Vector2 = Vector2()
@export var speed: float = 1000.0

func _process(delta):
	position += direction * speed * delta

func kill():
	queue_free()
