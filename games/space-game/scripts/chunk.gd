class_name Chunk

extends Node2D

var direction := Vector2()
var speed := 10.0

func _process(delta: float):
	position += direction * delta * speed

func set_texture(image: Image):
	$Sprite2D.texture = ImageTexture.create_from_image(image)
