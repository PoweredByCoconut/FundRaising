extends Control

@onready var option_button = $MarginContainer/VBoxContainer/WindowMode2 as OptionButton

const WINDOW_MODE_ARRAY: Array[String] = [
	"Full-Screen",
	"Windowed",
	"Borderless Window",
	"Borderless Full-Screen"
]

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	option_button.item_selected.connect(on_window_mode_selected)
	
func on_window_mode_selected(index: int) -> void:
	match index:
		0: #Fullscreen
			DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)
			DisplayServer.window_set_flag(DisplayServer.WINDOW_FLAG_BORDERLESS, false)
		1: #Windowed
			DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)
			DisplayServer.window_set_flag(DisplayServer.WINDOW_FLAG_BORDERLESS, false)
		2: #Borderless Window
			DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)
			DisplayServer.window_set_flag(DisplayServer.WINDOW_FLAG_BORDERLESS, true)
		3: #Borderless Fullscreen
			DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)
			DisplayServer.window_set_flag(DisplayServer.WINDOW_FLAG_BORDERLESS, true)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass


func _on_volume_value_changed(value: float) -> void:
	AudioServer.set_bus_volume_db(0, 10 * log(value / 100) / log(10))

func _on_check_box_toggled(toggled_on: bool) -> void:
	AudioServer.set_bus_mute(0, toggled_on)

func _on_back_pressed() -> void:
	get_tree().change_scene_to_file("res://games/space-game/scenes/main_menu.tscn")
