extends Node

@export var listener:AkListener2D 

#Banks Load
@export_group("Banks")
@export var music_bank:WwiseBank
@export var permanent_bank:WwiseBank

#Events Load
@export_group("Music Events")
@export var music_event:WwiseEvent
@export_group("Player Events")
@export var footsteps_event:WwiseEvent
@export_group("Interactable")
@export_subgroup("Laundry")
@export var laundry_pickup_clothes_event:WwiseEvent
@export var laundry_load_event:WwiseEvent
@export var laundry_running_event:WwiseEvent
@export_subgroup("Dishwashing")
@export var dish_start_event:WwiseEvent
@export_subgroup("Make the bed")
@export var make_bed_play_event:WwiseEvent
@export var make_bed_stop_event:WwiseEvent
@export_subgroup("Trash")
@export var trash_event:WwiseEvent

#States Load
@export_group("States")
@export_subgroup("Washing Machine States")
@export var washing_machine_state_group:WwiseStateGroup
@export var entrance_room_state:WwiseState
@export var bed_room_state:WwiseState
@export var outside_room_state:WwiseState
@export var bath_room_state:WwiseState
@export_subgroup("Bed States")
@export var bed_state_state_group:WwiseStateGroup
@export var bed_interacted_state:WwiseState
@export var bed_not_interacted_state:WwiseState
@export_subgroup("Mute Music")
@export var mute_music_state_group:WwiseStateGroup
@export var mute_music_true_state:WwiseState
@export var mute_music_false_state:WwiseState
@export_subgroup("Mute SFX")
@export var mute_sfx_state_group:WwiseStateGroup
@export var mute_sfx_true_state:WwiseState
@export var mute_sfx_false_state:WwiseState

#RTPC Load
@export_group("RTPC")
@export var percentage_bed_RTPC:WwiseRTPC

#Switch Load
@export_group("Switch")
@export_subgroup("Music Switch")
@export var music_switch_group:WwiseSwitchGroup
@export var title_music_switch:WwiseSwitch
@export var home_music_switch:WwiseSwitch
@export var phase1_music_switch:WwiseSwitch
@export var phase2_music_switch:WwiseSwitch
@export var phase3_music_switch:WwiseSwitch
@export var ending_music_switch:WwiseSwitch



func _ready():
	EventSystem.trigger_changed.connect(_on_trigger_changed)

func _on_trigger_changed(key: String, value: bool) -> void:
	if "cloth_picked_" in key and value:
		AudioSystem.post_event(laundry_pickup_clothes_event.id)
	if "washing_machine_full" in key and value:
		AudioSystem.post_event(laundry_load_event.id)
	if key == "bed_made" and value:
		AudioSystem.post_event(make_bed_stop_event.id,true)
	if key == "doors_locked" and value:
		AudioSystem.set_switch(music_switch_group.id,title_music_switch.id)
		AudioSystem.post_event(music_event.id,true)
	if key == "act_1" and value:
		AudioSystem.set_switch(music_switch_group.id,home_music_switch.id)
	if key == "washed_dishes" and value:
		AudioSystem.post_event(make_bed_stop_event.id) #cambiare evento

func _on_event_endend(key: int):
	if key == laundry_load_event.id:
		AudioSystem.post_event(laundry_running_event.id,true)
	if key == make_bed_play_event.id:
		AudioSystem.post_event(make_bed_stop_event.id,true)
