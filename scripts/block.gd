extends StaticBody2D

var destructable = false
signal destroyed(points: int)

func hit() -> void:
	if destructable:
		destroyed.emit(100)   # tell everyone it was destroyed and how many points
		queue_free()
