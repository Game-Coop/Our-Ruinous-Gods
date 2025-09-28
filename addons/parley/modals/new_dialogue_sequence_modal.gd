# Copyright 2024-2025 the Bisterix Studio authors. All rights reserved. MIT license.

@tool
class_name ParleyNewDialogueSequenceModal extends Window


#region DEFS
@onready var path_edit: LineEdit = %PathEdit
@onready var title_edit: LineEdit = %TitleEdit
@onready var choose_path_modal: FileDialog = %ChoosePathModal


# TODO: get this from config (note, see the Node inspector as well)
const default_current_dir: String = "res://dialogue_sequences"
const default_current_file: String = "new_dialogue.ds"


signal dialogue_ast_created(dialogue_ast: ParleyDialogueSequenceAst)
#endregion


#region LIFECYCLE
func display() -> void:
	show()
	if path_edit:
		path_edit.text = default_current_dir.path_join(default_current_file)
	if title_edit:
		title_edit.text = ""


func _exit_tree() -> void:
	if path_edit:
		path_edit.text = default_current_dir.path_join(default_current_file)
	if title_edit:
		title_edit.text = ""
#endregion


#region SIGNALS
func _on_file_dialog_file_selected(path: String) -> void:
	if path_edit:
		path_edit.text = path


func _on_choose_path_button_pressed() -> void:
	choose_path_modal.show()
	choose_path_modal.current_dir = default_current_dir
	choose_path_modal.current_file = default_current_file


func _on_cancel_button_pressed() -> void:
	hide()


func _on_create_button_pressed() -> void:
	if not path_edit or not title_edit:
		return
	hide()
	var dialogue_sequence_ast: ParleyDialogueSequenceAst = await ParleyUtils.file.create_new_resource(
		ParleyDialogueSequenceAst.new(title_edit.text),
		path_edit.text,
		get_tree().create_timer(30).timeout
	)
	if dialogue_sequence_ast:
		dialogue_ast_created.emit(dialogue_sequence_ast)
#endregion
