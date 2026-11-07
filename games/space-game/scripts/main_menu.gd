extends Control

func _ready() -> void:
	AudioPlayer.play_level_music()


func _on_play_pressed() -> void:
	get_tree().change_scene_to_file("res://games/space-game/scenes/game.tscn")


func _on_settings_pressed() -> void:
	get_tree().change_scene_to_file("res://games/space-game/scenes/setting.tscn")


func _on_credits_pressed() -> void:
	$CenterContainer/MainButtons.visible = false
	$CreditsMenu.visible = true


func _on_back_pressed() -> void:
	$CreditsMenu.visible = false
	$CenterContainer/MainButtons.visible = true


func _on_quit_pressed() -> void:
	get_tree().change_scene_to_file("res://menu/MainMenu.tscn")


func _on_instructions_pressed() -> void:
	get_tree().change_scene_to_file("res://games/space-game/scenes/instructions.tscn")
