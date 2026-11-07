extends Node2D

var focus_asteroid: Asteroid
var rad_speed: float = 100.0

var queue_fire = false

func _process(delta):
	if focus_asteroid != null:
		var target = atan2(focus_asteroid.position.y, focus_asteroid.position.x)
		
		rotation = lerp_angle(rotation, target, rad_speed * delta)
		
		if queue_fire:
			fire()
			queue_fire = false

func set_focus(asteroid: Asteroid):
	focus_asteroid = asteroid

func enable():
	$CooldownTimer.start()
	visible = true

func on_cooldown():
	queue_fire = true
	$CooldownTimer.wait_time = 1.0

func fire():
	var bullet_scene: PackedScene = load("res://games/space-game/scenes/bullet.tscn")
	var bullet: Bullet = bullet_scene.instantiate()
	$LaserGunAudio.play()
	bullet.rotation = rotation
	bullet.direction = Vector2.from_angle(rotation)
	add_sibling(bullet)
