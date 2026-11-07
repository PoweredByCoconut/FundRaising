extends Control

func _ready():
	$pauseBg.visible = false

func _process(_delta):
	if Input.is_action_just_pressed("Pause"):
		if get_tree().paused:
			_on_resume_pressed()
		else:
			_on_pause_pressed()

func _on_pause_pressed():
	get_tree().paused = true
	$pauseBg.visible = true

func _on_resume_pressed():
	get_tree().paused = false
	$pauseBg.visible = false

func _on_menu_pressed():
	get_tree().paused = false
	get_tree().change_scene_to_file("res://games/space-game/scenes/main_menu.tscn")
