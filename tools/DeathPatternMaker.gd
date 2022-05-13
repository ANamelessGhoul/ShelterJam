tool
extends Node

export (String, "None", "Speedup", "Revive", "Bounce", "WallBreak") \
var pivotType: String

export (String, FILE) var savePath : String = "res://Pattern.tres"

export var run : bool setget _set_run

func _set_run(value):
	run = false
	property_list_changed_notify()
	create_pattern()

func _ready():
	if Engine.editor_hint:
		return
	create_pattern()


func create_pattern():
	var tilemap: TileMap = $TileMap
	var tile_set: TileSet = tilemap.tile_set
	
	var skills: Dictionary = {}
	var pivot: Vector2 = Vector2.ZERO
	
	for tile_id in tile_set.get_tiles_ids():
		var tile_name = tile_set.tile_get_name(tile_id)
		if tile_name == "Pivot":
			var pivot_positions = tilemap.get_used_cells_by_id(tile_id)
			pivot = pivot_positions[0]
			pivot_warning(pivot_positions, pivot)
			
			if pivotType != "None":
				add_group(skills, pivotType)
				skills[pivotType].append_array(pivot_positions)
		else:
			add_group(skills, tile_name)
			var positions = tilemap.get_used_cells_by_id(tile_id)
			skills[tile_name].append_array(positions)
		
	

	
	var patternResource = DeathPattern.new()
	patternResource.skills = skills
	patternResource.origin = pivot
	
	var e = ResourceSaver.save(savePath, patternResource)
	if e != OK:
		print("Error %s" % e)
	else:
		print("Succesfully created pattern at %s" % savePath)

func add_group(skills: Dictionary, group: String):
	if skills.has(group):
		return
	skills[group] = []

func pivot_warning(pivot_positions: Array, pivot: Vector2):
	if pivot_positions.size() > 1:
		printerr("More than one pivot found. " + 
			"Pivot found at %s will be used as pivot" % pivot + 
			"and the others will be used as speedup tiles.")

