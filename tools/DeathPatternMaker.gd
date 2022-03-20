extends Node

export var includePivot: bool = true

export var savePath : String = "res://Pattern.tres"

func _ready():
	var tilemap: TileMap = $TileMap
	var tile_set: TileSet = tilemap.tile_set
	var pivotIndex = tile_set.find_tile_by_name("Pivot")
	var tileIndex = tile_set.find_tile_by_name("Tile")
	
	
	var pivot = tilemap.get_used_cells_by_id(pivotIndex)[0]
	var positions = tilemap.get_used_cells_by_id(tileIndex)
	
	if includePivot:
		positions.append(pivot)
	
	var patternResource = DeathPattern.new()
	patternResource.positions = positions
	patternResource.origin = pivot
	
	var e = ResourceSaver.save(savePath, patternResource)
	if e != OK:
		print("Error %s" % e)
