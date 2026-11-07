extends Sprite2D

@onready var AniPlay1 = $AniPlay1
@onready var AniPlay2 = $AniPlay2
@onready var AniPlay3 = $AniPlay3

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	AniPlay1.play("background_switch_1")
	await get_tree().create_timer(20).timeout
	AniPlay2.play("background_switch_2")
	await get_tree().create_timer(20).timeout
	AniPlay3.play("background_switch_3")
	await get_tree().create_timer(20).timeout
