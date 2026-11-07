extends Node2D

# define variables needed by this class
@export var asteroid_spawn_distance: int = 750
@export var asteroid_scene: PackedScene
@export var crater_radius: int = 15

signal game_over(asteroids: int, aliens: int, time: int)

# get a random number generator
var rng = RandomNumberGenerator.new()

var asteroids: Array[Asteroid] = []
var aliens: Array[Alien] = []

var num_asteroids := 0
var num_aliens := 0
var phase := 0
var time := 0

# define a function to spawn asteroids, this is tied to the AsteroidSpawnTimer timeout signal
func spawn_asteroid():
	# generate a random unit vector for the direction we want to spawn the asteroid
	var direction = random_unit_vector()
	
	# convert that direction to a position
	var asteroid_position = asteroid_spawn_distance * random_unit_vector()
	
	# instantiate the asteroid to spawn it
	var asteroid : Asteroid = asteroid_scene.instantiate()
	
	# set the asteroid's position
	asteroid.position = asteroid_position
	
	# set the asteroid's direction (difference between us and the asteoid)
	asteroid.direction = (position + random_in_unit_sphere() * ($Earth.texture.get_width() / 2) - asteroid.position).normalized()
	
	asteroid.blocked.connect(increment_asteroids)
	
	# add the asteroid to our parent (the root node)
	add_sibling(asteroid)
	
	asteroids.append(asteroid)
	
	if phase == 0:
		$AsteroidSpawnTimer.wait_time *= 0.99
		if num_asteroids >= 50:
			phase += 1
			$AsteroidSpawnTimer.wait_time = 0.7
			$AlienSpawnTimer.start()
			$Ship.enable()

func _process(_delta):
	var min_dist: float = 1000000
	var closest_asteroid: Asteroid
	for asteroid in asteroids:
		if is_instance_valid(asteroid):
			var dist = asteroid.position.length_squared()
			if dist < min_dist:
				min_dist = dist
				closest_asteroid = asteroid
	$Ship.set_focus(closest_asteroid)
	
	for alien in aliens:
		if is_instance_valid(alien):
			if alien.distance <= 120:
				alien.retreat = true
				bore_texture(alien.rotation)
				recompute_collision_shape()
			

func spawn_alien():
	var alien_scene: PackedScene = load("res://games/space-game/scenes/alien.tscn")
	var alien: Alien = alien_scene.instantiate()
	$alienAudio.play()
	alien.rotation = aliens.size() * PI / 6
	alien.killed.connect(increment_aliens)
	add_sibling(alien)
	aliens.append(alien)

func increment_asteroids():
	num_asteroids += 1

func increment_aliens():
	num_aliens += 1

func increment_timer():
	time += 1

func random_unit_vector() -> Vector2:
	return Vector2.from_angle(rng.randf_range(0, 2 * PI))

func random_in_unit_sphere() -> Vector2:
	return Vector2.from_angle(rng.randf_range(0, 2 * PI)) * rng.randf()

func on_hit(body: Area2D):
	if body.get_parent() is Asteroid:
		var asteroid = body.get_parent() as Asteroid
		asteroid.kill()
		
		var camera := get_viewport().get_camera_2d()
		if camera is CameraShake:
			camera.apply_shake()
		
		$earthCrumbleAudio.play(5)
		hollow_texture(asteroid.position)
		recompute_collision_shape()

func on_lose_hit(body: Area2D):
	if body.get_parent() is Asteroid:
		emit_signal("game_over", num_asteroids, num_aliens, time)

func hollow_texture(crater_position: Vector2):
	var image: Image = $Earth.texture.get_image()
	var image_size: Vector2 = $Earth.texture.get_size()
	
	for i in range(-crater_radius, crater_radius):
		for j in range(-crater_radius, crater_radius):
			var pos := crater_position + image_size / 2 + Vector2(j, i)
			if i * i + j * j < crater_radius * crater_radius and 0 <= pos.x and pos.x < image_size.x and 0 <= pos.y and pos.y < image_size.y:
				image.set_pixelv(pos, Color(0, 0, 0, 0))
	
	update_texture(image)

func bore_texture(direction: float):
	var image: Image = $Earth.texture.get_image()
	var image_size: Vector2 = $Earth.texture.get_size()
	
	var length = $Earth.texture.get_width() / 2
	for i in range(length):
		for j in range(-1, 1):
			var pos = Vector2.from_angle(direction) * i + Vector2(j, 0) + image_size / 2
			image.set_pixelv(pos, Color(0, 0, 0, 0))
	
	update_texture(image)

func recompute_collision_shape():
	var texture: Texture = $Earth.texture
	
	for child in $Area2D.get_children():
		child.queue_free()
	
	var bm = BitMap.new()
	bm.create_from_image_alpha(texture.get_image())
	var rect = Rect2(0, 0, texture.get_width(), texture.get_height())
	var polygons = bm.opaque_to_polygons(rect, 1)
	
	var largestArea := 0.0
	var largestCollision := CollisionPolygon2D.new()
	var largestIndex := 0
	
	var collision_list: Array[CollisionPolygon2D] = []
	
	for i in range(polygons.size()):
		var my_collision = CollisionPolygon2D.new()
		my_collision.set_polygon(polygons[i])
		my_collision.position -= Vector2((texture.get_width() / 2), (texture.get_height() / 2)) * scale.x
		
		var area = polygon_area(polygons[i])
		if(area > largestArea):
			largestArea = area
			largestCollision = my_collision
			largestIndex = i
		
		collision_list.append(my_collision)
	
	$Area2D.call_deferred("add_child", largestCollision)
	collision_list.remove_at(largestIndex)
	
	if collision_list.size() > 0:
		separate_polygons(collision_list)

func separate_polygons(collision_list: Array[CollisionPolygon2D]):
	var image_data: Image = $Earth.texture.get_image()
	var width = image_data.get_width()
	var height = image_data.get_height()
	
	for polygon in collision_list:
		var new_image = Image.create(width, height, false, image_data.get_format())
		
		for y in range(height):
			for x in range(width):
				var pos = Vector2(x, y)
				
				if Geometry2D.is_point_in_polygon(pos, polygon.polygon):
					new_image.set_pixel(x, y, image_data.get_pixel(x, y))
					image_data.set_pixel(x, y, Color(0, 0, 0, 0))
		
		var chunk_scene: PackedScene = load("res://games/space-game/scenes/chunk.tscn")
		var chunk: Chunk = chunk_scene.instantiate()
		
		chunk.set_texture(new_image)
		chunk.direction = random_unit_vector()
		add_sibling(chunk)
		
		update_texture(image_data)

func update_texture(image: Image):
	$Earth.texture = ImageTexture.create_from_image(image)
	
	var damaged_earth: = $DamagedEarth as Sprite2D;
	var de_material = damaged_earth.material
	if de_material is ShaderMaterial:
		de_material.set_shader_parameter("earth", $Earth.texture)

# use the Gauss's Shoelace function to calculate area
func polygon_area(polygon: PackedVector2Array) -> float:
	var area := 0.0
	var num_vertices := polygon.size()
	
	if num_vertices < 3:
		return 0.0
	
	for q in range(num_vertices):
		var p = (q - 1 + num_vertices) % num_vertices
		area += polygon[q].cross(polygon[p])
	
	return abs(area) * 0.5
