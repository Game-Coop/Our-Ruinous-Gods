extends ParleyFactInterface


func evaluate(ctx: ParleyContext, _values: Array) -> bool:
	return ParleyFacts.GetValue("isAlive") == true

# add this if using match nodes. its not necessary but it will draw these values out and automatically add as a case.
# enum DifficultyLevel {
# 	EASY,
# 	NORMAL,
# 	HARD,
# }
# func available_values() -> Array[String]:
# 	return [
# 		"bip",
# 		"bup",
# 		"bop",
# 	]
