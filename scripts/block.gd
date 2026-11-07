extends StaticBody2D

signal destroyed(points: int)

func hit() -> void:
	destroyed.emit(100)   # tell everyone it was destroyed and how many points
	queue_free()
