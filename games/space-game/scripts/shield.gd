class_name Shield

extends Node2D

@export var rad_speed: float = 20.0
var mouse_active := true

func _process(delta: float):
	if mouse_active:
		var mouse_pos := get_viewport().get_mouse_position() - Vector2(get_viewport().size) / 2
		var target = atan2(mouse_pos.y, mouse_pos.x)
		
		rotation = lerp_angle(rotation, target, rad_speed * delta)
	
	if Input.is_action_pressed("left"):
		rotation = fmod(rotation - rad_speed * delta, 2 * PI)
		mouse_active = false
	if Input.is_action_pressed("right"):
		rotation = fmod(rotation + rad_speed * delta, 2 * PI)
		mouse_active = false
	
	if Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT):
		mouse_active = true
	
	if (Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT) or Input.is_action_just_pressed("spacebar")) and $BulletCooldown.time_left == 0:
		var shield_bullet_scene: PackedScene = load("res://games/space-game/scenes/shield_bullet.tscn")
		var shield_bullet: ShieldBullet = shield_bullet_scene.instantiate()
		shield_bullet.direction = Vector2.from_angle(rotation)
		shield_bullet.rotation = rotation
		add_sibling(shield_bullet)
		$BulletCooldown.start()
	
	var mat = $Shield2D.material
	if mat is ShaderMaterial:
		mat.set_shader_parameter("ready", $BulletCooldown.time_left == 0)


func _on_area_2d_area_entered(_area: Area2D) -> void:
	$shieldBlockAudio.play(1.25)
