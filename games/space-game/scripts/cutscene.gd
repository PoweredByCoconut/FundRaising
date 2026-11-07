extends Control

func _process(_delta):
	if Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT):
		_on_video_stream_player_finished()

func _on_video_stream_player_finished() -> void:
	get_tree().change_scene_to_file("res://games/space-game/scenes/main_menu.tscn")
