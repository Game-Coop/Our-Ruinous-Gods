extends Node

@export var bus = "Sfx"
@export var mute = false
@export var master_volume : int
@export var music_volume : int
@export var sfx_volume : int
@export var ambience_volume : int
@export var audio_directory : String
var audio_files : Dictionary

var playing_events : Dictionary = {}
var object_playing: Dictionary	= {}

func _ready():
	#Wwise integration
	Wwise.load_bank(AudioHolder.music_bank.name)	
	Wwise.load_bank(AudioHolder.permanent_bank.name)
	Wwise.register_game_obj(self, self.name)

func set_starting_ui_volume() -> void:
	EventSystem.set_volume.emit("ambience_volume", ambience_volume)
	EventSystem.set_volume.emit("master_volume", master_volume)
	EventSystem.set_volume.emit("music_volume", music_volume)
	EventSystem.set_volume.emit("sfx_volume", sfx_volume)
	

func set_volumes_value(name: String, value: float) -> void :
	print("[Audio] set_volume_value - channel:", name, ",value:", value)
	
	if name.is_empty():
		return
	
	var dictionary_volumes = get_volume_parameters()
	print(dictionary_volumes)
	Wwise.set_rtpc_value_id(dictionary_volumes[name], value, null)
	

func post_event(event_id : int, single_event : bool = false, gameObject:Node=self):
	#if this need to be a single event, if it already in the map do not start it again
	if single_event:
		if playing_events.has(event_id):
			return
	playing_events[event_id] = Wwise.post_event_id_callback(event_id, AkUtils.AK_END_OF_EVENT, gameObject, event_end)
	print("[Audio] Playing event", playing_events)
	object_playing[event_id] = gameObject
	print("[Audio] Object playing", object_playing)
func event_end(data):
	print("[Audio] An event has ended:", data["eventID"])
	#{ "callback_type": 1, "eventID": 2462354508, "gameObjID": 129771767101, "playingID": 8 }
	AudioHolder._on_event_endend(data["eventID"])
	if playing_events.has(data["eventID"]):		
		playing_events.erase(data["eventID"])
	print("post_event", playing_events)

func stop_event(event_id, fade_time : int, curve : AkUtils.AkCurveInterpolation) :
	if not playing_events.keys().has(event_id):
		printerr("[Audio] Playing event_id not found in dictionary id:", event_id)
		return
	var instance_id : int = playing_events[event_id]
	if instance_id == null:
		printerr("[Audio] Playing event_id not found id:", event_id)
		return
	Wwise.stop_event(instance_id, fade_time, curve)
	#if the event was stopped clear the key from the map
	playing_events.erase(event_id)
	print("[Audio] stop_event", playing_events)

func set_state(group_state_id, name_state_id):
	Wwise.set_state_id(group_state_id,name_state_id)

func set_rtpc(rtpc_id, name_state_id, gameObject:Node=null):
	Wwise.set_rtpc_value_id(rtpc_id,name_state_id,gameObject)
	
func set_switch(switch_id, name_switch_id, gameObject:Node=self):
	Wwise.set_switch_id(switch_id,name_switch_id,gameObject)
	
func get_volume_parameters() -> Dictionary:
	return {
		"sfx_volume": AK.GAME_PARAMETERS.SFX_VOLUME,
		"master_volume": AK.GAME_PARAMETERS.MASTER_VOLUME,
		"ambience_volume": AK.GAME_PARAMETERS.AMBIENCE_VOLUME,
		"music_volume": AK.GAME_PARAMETERS.MUSIC_VOLUME,
	}
