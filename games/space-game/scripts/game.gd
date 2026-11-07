extends Node2D

func game_over(asteroids: int, ufos: int, time: int):
	$PauseMenu.visible = false
	$GameOver.visible = true
	get_tree().paused = true
	$GameOver.game_over(asteroids, ufos, time)
