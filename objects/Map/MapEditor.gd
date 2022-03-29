tool
extends Node

const object_dict = \
{
	"Character OBJ": "res://objects/Character/Character.tscn",
	"Flag OBJ": "res://objects/Flag/Flag.tscn"
}


var tile_id_to_tile_name: Dictionary = {}

var tile_map: TileMap

func _ready():
	if not Engine.editor_hint:
		queue_free()
	
	tile_map = get_parent()
	tile_id_to_tile_name = {}
	var tile_set := tile_map.tile_set
	for tile_id in tile_set.get_tiles_ids():
		var tile_name = tile_set.tile_get_name(tile_id)
		if tile_name.ends_with("OBJ"):
			tile_id_to_tile_name[tile_id] = tile_name

func _process(_delta):
	var cell_size = tile_map.cell_size
	
	var world_offset = Vector2(0, cell_size.y / 2)
	
	for tile_id in tile_id_to_tile_name:
		var cells = tile_map.get_used_cells_by_id(tile_id)
		var object_path = object_dict[tile_id_to_tile_name[tile_id]]
		var prefab = load(object_path)
		for cell in cells:
			var object = prefab.instance()
			tile_map.add_child(object)
			object.global_position += tile_map.map_to_world(cell) + world_offset
			object.set_owner(get_tree().edited_scene_root)
			tile_map.set_cellv(cell, -1)
			
