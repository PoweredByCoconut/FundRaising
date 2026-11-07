extends CharacterBody2D

var SPEED = 300.0

func _physics_process(delta: float) -> void:
	var direction := Input.get_axis("left", "right")
	velocity.x = direction * SPEED
	velocity.y = 0
	move_and_slide()

	# Lock vertical position so the ball can never push it down or up
	# Change 500 to whatever your paddle’s real Y position is
	position.y = 233
