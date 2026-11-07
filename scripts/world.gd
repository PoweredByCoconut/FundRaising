extends Node2D   # or whatever your root is

@onready var score_label: Label = $"Score Label"
@onready var live_counter: Label = $"Live Counter"

var score: int = 0
var lifes: int = 3

func _ready() -> void:
	# Connect to every block that currently exists
	for block in get_tree().get_nodes_in_group("blocks"):
		block.destroyed.connect(_on_block_destroyed)

	# If you spawn blocks later, also connect them when you instance them

func _on_block_destroyed(points: int) -> void:
	score += points
	score_label.text = "Score: %d" % score

func _on_death() -> void:
	lifes-=1
	if lifes>0:
		live_counter.text = "Lives: %d" % lifes
	else:
		#game over
		pass
