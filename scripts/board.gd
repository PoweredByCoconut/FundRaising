extends StaticBody2D

@onready var world: Node2D = $".."
@onready var ball: CharacterBody2D = $"../ball"

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


func _on_deathbox_body_entered(body: Node2D) -> void:
	print("deathbox entered")
	ball.launched = false
	world._on_death()
	
