extends Control

func _on_restart_pressed() -> void:
	get_tree().paused = false
	get_tree().change_scene_to_file("res://games/space-game/scenes/game.tscn")


func _on_main_menu_pressed() -> void:
	get_tree().paused = false
	get_tree().change_scene_to_file("res://games/space-game/scenes/main_menu.tscn")

func game_over(asteroids: int, ufos: int, time: int):
	var asteroid_label: Label = $PanelContainer/MarginContainer/VBoxContainer/Asteroids
	asteroid_label.text += " " + str(asteroids)
	
	var alien_label: Label = $PanelContainer/MarginContainer/VBoxContainer/Aliens
	alien_label.text += " " + str(ufos)
	
	var minute = int(time / 60)
	var sec = time % 60
	var time_label: Label = $PanelContainer/MarginContainer/VBoxContainer/TimeSurvived
	time_label.text += " " + str(minute) + " minutes and " + str(sec) + " seconds"
