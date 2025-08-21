# Copyright 2024-2025 the Bisterix Studio authors. All rights reserved. MIT license.

@tool
class_name ParleyRuntime extends Node


#region DEFS
const Constants = preload('./constants.gd')
const Settings = preload('./settings.gd')
const DialogueSequenceAst = preload('./models/dialogue_sequence_ast.gd')
const NodeAst = preload('./models/node_ast.gd')
const Context = preload('./models/parley_context.gd')
const Utils = preload("./utils/parley_util.gd")


var version: String = Constants.VERSION

# TODO: consider moving the get_instance back here
#endregion


#region GAME
## Run a dialogue session with the provided Dialogue Sequence AST
## Example: parley_runtime.run_dialogue(ctx, dialogue_sequence_ast)
func run_dialogue(ctx: Context, dialogue_sequence_ast: DialogueSequenceAst, start_node: NodeAst = null) -> Node:
	# TODO: maybe pass this in instead of getting from the engine - gives us a bit more flexibility
	var current_scene: Node = _get_current_scene()
	var dialogue_balloon_path: String = Settings.get_setting(Constants.DIALOGUE_BALLOON_PATH)
	if not ResourceLoader.exists(dialogue_balloon_path):
		print_rich(Utils.log.info_msg("Dialogue balloon does not exist at: %s. Stopping..."))
		return
	var dialogue_balloon_scene: PackedScene = load(dialogue_balloon_path)
	var balloon: Node = dialogue_balloon_scene.instantiate()
	current_scene.add_child(balloon)
	if not dialogue_sequence_ast:
		push_error(Utils.log.error_msg("No active Dialogue AST set, exiting."))
		return balloon
	if balloon.has_method(&"start"):
		@warning_ignore("UNSAFE_METHOD_ACCESS") # Covered by the if statement
		balloon.start(ctx, dialogue_sequence_ast, start_node)
	else:
		# TODO: add translation for error here
		assert(false, "Dialogue balloon is missing the `start` method can cannot run the Dialogue Sequence")
	return balloon


func _get_current_scene() -> Node:
	@warning_ignore("UNSAFE_PROPERTY_ACCESS")
	var current_scene: Node = Engine.get_main_loop().current_scene
	if current_scene == null:
		@warning_ignore("UNSAFE_PROPERTY_ACCESS")
		@warning_ignore("UNSAFE_METHOD_ACCESS")
		current_scene = Engine.get_main_loop().root.get_child(Engine.get_main_loop().root.get_child_count() - 1)
	return current_scene
#endregion
