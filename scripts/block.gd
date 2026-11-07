extends StaticBody2D

@onready var destructable: bool = true
@onready var blockType: int = get_meta("blocktype", 0)
@onready var anim: AnimatedSprite2D = $AnimatedSprite2D

signal destroyed(points: int)

func _ready() -> void:
	match blockType:
		0: anim.play_backwards("redbreak")
		1: anim.play_backwards("greenbreak")
		2: anim.play_backwards("bluebreak")
		3: anim.play_backwards("purpbreak")
		4: anim.play_backwards("yellowbreak")
		5: anim.play_backwards("orangebreak")
		6:
			print("wood")
			destructable = false
		_:
			print("Unknown block type:", blockType)

func hit() -> void:
	if not destructable:
		return

	# Disable collision so the ball doesn’t hit it again
	$CollisionShape2D.set_deferred("disabled", true)

	destroyed.emit(100)

	# Play the break animation
	match blockType:
		0: anim.play("redbreak")
		1: anim.play("greenbreak")
		2: anim.play("bluebreak")
		3: anim.play("purpbreak")
		4: anim.play("yellowbreak")
		5: anim.play("orangebreak")

	# Wait until the animation finishes, then remove the block
	await anim.animation_finished
	queue_free()    # then free yourself
