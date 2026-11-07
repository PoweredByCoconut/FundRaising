class_name Asteroid

extends Node2D

# define variables we need
var direction: Vector2
@export var textures: Array[Texture2D]
@export var speed: float = 250.0

signal blocked

var rng = RandomNumberGenerator.new()

func _ready():
	$AsteroidSprite.texture = textures[rng.randi_range(0, textures.size() - 1)]
	look_at(Vector2.ZERO)
	speed *= rng.randf_range(0.5, 1.5 )

# function to update every frame
func _process(delta):
	# move in the direction we want to move
	position += direction * speed * delta

func kill():
	speed = 0
	for child in get_children():
		if child is Node2D:
			child.visible = false
	$Explosion.visible = true
	$Explosion/AnimationPlayer.play("explosion_animation")
	$ImpactAudio.play(0.5)

func despawn(anim_name: StringName):
	if anim_name == "explosion_animation":
		queue_free()

func hit(area: Area2D):
	if area.get_parent() is Shield:
		kill()
		
		emit_signal("blocked")
		
		var camera := get_viewport().get_camera_2d()
		if camera is CameraShake:
			camera.apply_shake(10)
	if area.get_parent() is Bullet:
		kill()
		
		emit_signal("blocked")
		
		var camera := get_viewport().get_camera_2d()
		if camera is CameraShake:
			camera.apply_shake(5)
	 
