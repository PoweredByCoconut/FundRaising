extends Node2D


# At the top with your other variables
const BLOCK_SCENE = preload("res://games/block-destroyer/scenes/block.tscn")  # ← change path if needed

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
var possible_types: Array[int] = [0, 1, 2, 3, 4, 5]
@onready var score_label: Label = $"Score Label"
@onready var lives_container: HBoxContainer = $LivesContainer   # ← your new container
@onready var ball: CharacterBody2D = $ball
@onready var paddle: CharacterBody2D = $paddle
@onready var blocks_node: Node = $blocks
@onready var game_over_screen: Control = $GameOverScreen
@onready var game_over_label: Label = $GameOverScreen/GameOverLabel
@onready var final_score_label: Label = $GameOverScreen/FinalScoreLabel
@export var ball_life_texture: Texture2D

var score: int = 0
var lives: int = 2
const MAX_LIVES := 5
var bricks_left: int = 0

func _ready() -> void:
	# Clear any blocks that might already be in the scene (the reference ones)
	clear_board()
	
	# Fill the board for the first time
	refill_board()
	update_lives_display()
	update_ui()

func _on_block_destroyed(points: int) -> void:
	ball.SPEED += 2
	paddle.SPEED += 0.2
	score += points
	update_ui()

	bricks_left -= 1
	print("Bricks left: ", bricks_left)          # ← debug

	if bricks_left <= 0:
		level_cleared()


func is_board_empty() -> bool:
	return bricks_left <= 0


func level_cleared() -> void:
	print("Board cleared!")
	score += 1000
	update_ui()

	ball.launched = false

	clear_board()
	await get_tree().create_timer(0.15).timeout   # safer than process_frame
	refill_board()


func clear_board() -> void:
	for child in blocks_node.get_children():
		child.queue_free()
	bricks_left = 0


func refill_board() -> void:
	var row_types = possible_types.duplicate()
	row_types.shuffle()

	bricks_left = 0

	for row in range(rows):
		var type: int = row_types[row % row_types.size()]

		for col in range(columns):
			var block = BLOCK_SCENE.instantiate()
			block.set_meta("blocktype", type)

			var x = start_position.x + col * (brick_width + horizontal_gap)
			var y = start_position.y + row * (brick_height + vertical_gap)
			block.position = Vector2(x, y)

			blocks_node.add_child(block)
			block.destroyed.connect(_on_block_destroyed)

			bricks_left += 1

	print("Board refilled with ", bricks_left, " blocks")
func update_ui() -> void:
	score_label.text = "Score: %d" % score
	# You can remove or hide the old "Lives: X" label now

func update_lives_display() -> void:
	# Clear old icons
	for child in lives_container.get_children():
		child.queue_free()

	# Create one icon per remaining life
	for i in range(lives):
		var icon = TextureRect.new()
		icon.texture = ball_life_texture
		icon.expand_mode = TextureRect.EXPAND_IGNORE_SIZE
		icon.stretch_mode = TextureRect.STRETCH_KEEP_ASPECT_CENTERED
		icon.custom_minimum_size = Vector2(24, 24)   # size of each life icon
		lives_container.add_child(icon)


func _on_death() -> void:
	lives -= 1
	update_lives_display()

	if lives < 0:
		game_over()
	else:
		ball.launched = false
		# reset ball position here if you have a function for it


func game_over() -> void:
	# Stop gameplay
	ball.launched = false
	ball.velocity = Vector2.ZERO

	# Clear the board
	clear_board()

	# Hide normal UI
	score_label.text = ""
	lives_container.visible = false

	# Show Game Over screen
	final_score_label.text = "SCORE: %d" % score
	game_over_screen.visible = true

	# Optional: pause the game
	# get_tree().paused = true
	
func _process(_delta: float) -> void:
	if game_over_screen.visible and Input.is_action_just_pressed("spacebar"):  # Enter / Space
		restart_game()


func restart_game() -> void:
	get_tree().reload_current_scene()
