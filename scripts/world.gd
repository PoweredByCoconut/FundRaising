extends Node2D


# At the top with your other variables
const BLOCK_SCENE = preload("res://scenes/block.tscn")  # ← change path if needed

# Easy to tweak
@export var rows: int = 6
@export var columns: int = 8
@export var start_position: Vector2 = Vector2(-207, -175)   # top-left of the first brick
@export var brick_width: float = 56
@export var brick_height: float = 20
@export var horizontal_gap: float = 0
@export var vertical_gap: float = 5

# Which blocktype each row should use (top → bottom)
# 0=red, 1=green, 2=blue, 3=purp, 4=yellow, 5=orange, 6=wood
var row_types: Array[int] = [0, 1, 2, 3, 4, 5]
@onready var score_label: Label = $"Score Label"
@onready var live_counter: Label = $"Live Counter"
@onready var ball: CharacterBody2D = $ball
@onready var paddle: CharacterBody2D = $paddle
@onready var blocks_node: Node = $blocks

var score: int = 0
var lives: int = 3

func _ready() -> void:
	# Clear any blocks that might already be in the scene (the reference ones)
	clear_board()
	
	# Fill the board for the first time
	refill_board()
	
	update_ui()

func _on_block_destroyed(points: int) -> void:
	ball.SPEED += 2
	paddle.SPEED += 0.2
	score += points
	update_ui()

	# Wait one frame so queue_free() has finished
	await get_tree().process_frame

	if is_board_empty():
		level_cleared()

func is_board_empty() -> bool:
	return blocks_node.get_child_count() == 0

func level_cleared() -> void:
	print("Board cleared!")
	score += 1000
	update_ui()

	ball.launched = false
	# ball.velocity = Vector2.ZERO   # optional

	clear_board()
	await get_tree().process_frame   # make sure old blocks are gone

	refill_board()


func clear_board() -> void:
	for child in blocks_node.get_children():
		child.queue_free()


func refill_board() -> void:
	for row in range(rows):
		var type: int = row_types[row] if row < row_types.size() else 0

		for col in range(columns):
			var block = BLOCK_SCENE.instantiate()

			# Set the metadata BEFORE adding to the tree
			block.set_meta("blocktype", type)

			# Position
			var x = start_position.x + col * (brick_width + horizontal_gap)
			var y = start_position.y + row * (brick_height + vertical_gap)
			block.position = Vector2(x, y)

			blocks_node.add_child(block)

			# Connect the signal
			block.destroyed.connect(_on_block_destroyed)

	print("Board refilled with ", blocks_node.get_child_count(), " blocks")

func _on_death() -> void:
	lives -= 1
	update_ui()

	if lives <= 0:
		# Game over logic
		print("Game Over")
		# get_tree().paused = true
		# or show game over screen
	else:
		# Reset ball
		ball.launched = false
		# ball.reset_position()  # if you have this function

func update_ui() -> void:
	score_label.text = "Score: %d" % score
	live_counter.text = "Lives: %d" % lives
