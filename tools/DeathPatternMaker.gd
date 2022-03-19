extends Node

export var includePivot: bool = true

export var savePath : String = "res://Pattern.tres"

func _ready():
	var tilemap: TileMap = $TileMap
	var pivot = tilemap.get_used_cells_by_id(0)[0]
	var positions = tilemap.get_used_cells_by_id(1)
	
	if includePivot:
		positions.append(pivot)
	
	var patternResource = DeathPattern.new()
	patternResource.positions = positions
	patternResource.origin = pivot
	
	var e = ResourceSaver.save(savePath, patternResource)
	if e != OK:
		print("Error %s" % e)
