extends CharacterBody2D

var SPEED = 200.0

# Whether the ball is launched or not
var launched = false

func _ready():
	# Initially set velocity to zero (ball sits on paddle)
	velocity = Vector2.ZERO

func _physics_process(delta: float) -> void:
	if not launched:
		# If not launched, follow the paddle position
		var paddle = get_parent().get_node("paddle")
		position = paddle.position + Vector2(0, -20)  # Position above paddle
	else:
		var motion = velocity * delta
		var collision = move_and_collide(motion)

		if collision:
			var collider = collision.get_collider()

			# === Paddle handling (keep what you already have) ===
			if collider.name == "paddle" or collider is CharacterBody2D:
				var offset = (global_position.x - collider.global_position.x) / (collider.get_node("CollisionShape2D").shape.size.x * 0.5)
				offset = clamp(offset, -1.0, 1.0)
				velocity = Vector2(offset, -1).normalized() * SPEED

			# === Block handling ===
			elif collider.has_method("hit"):
				collider.hit()                    # tell the block it was hit
				velocity = velocity.bounce(collision.get_normal())
				velocity = velocity.normalized() * SPEED

			# === Walls ===
			else:
				velocity = velocity.bounce(collision.get_normal())
				velocity = velocity.normalized() * SPEED
func launch():
	# Launch the ball with initial velocity
	if not launched:
		launched = true
		velocity = Vector2(1, -1).normalized() * SPEED

func _input(event):
	if event.is_action_pressed("spacebar") and not launched:
		launch()
