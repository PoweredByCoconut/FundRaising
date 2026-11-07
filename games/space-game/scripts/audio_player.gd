extends AudioStreamPlayer

const level_music = preload("res://games/space-game/Music/boom-baddie-konstantin-garbuzyuk-main-version-42706-01-45.mp3")

func _play_music(music: AudioStream):
	if stream == music:
		return
	stream = music
	play()
	
func play_level_music():
	_play_music(level_music)
