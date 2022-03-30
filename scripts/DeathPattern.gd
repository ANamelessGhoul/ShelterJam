extends Resource
class_name DeathPattern

export var origin: Vector2

export var skills: Dictionary

func get_skills_rotated(rotate_quarter: int, offset: Vector2 = Vector2.ZERO) -> Dictionary:
	var rotation := Transform2D()
	rotation = rotation.rotated(deg2rad(90) * rotate_quarter)
	
	
	var rotated_skills := {}
	for key in skills:
		var arr = []
		for position in skills[key]:
			var final_position: Vector2 = rotation.xform(position - origin) + offset
			arr.append(final_position.round())
		rotated_skills[key] = arr
	
	return rotated_skills
	
