
class_name Alien

extends Node2D

var distance: float = 500
var rot_speed: float = 250
var speed = 30
var retreat = false

signal killed

func _process(delta: float):
	$Sprite2D.position = Vector2.from_angle(rotation) * distance
	$Sprite2D.rotation = rotation + PI / 2
	$Area2D.position = Vector2.from_angle(rotation) * distance
	if !retreat:
		rotation += (rot_speed * delta) / distance
		if distance > 120:
			distance -= speed * delta
	else:
		$DespawnTimer.start()
		distance += speed * 12 * delta

func hit(area: Area2D):
	if area.get_parent() is ShieldBullet:
		emit_signal("killed")
		queue_free()

func despawn():
	queue_free()
