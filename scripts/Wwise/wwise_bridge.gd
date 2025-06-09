extends Node
class_name wwise_bridge

func post_event_id(event_id: int, game_object: Node) -> int:
	return Wwise.post_event_id(event_id, game_object)

func post_event_id_callback(event_id: int, callback_mask: int, game_object: Node, callback: Callable) -> int:
	return Wwise.post_event_id_callback(event_id, callback_mask, game_object, callback)

func stop_event(playing_id: int, fade_time_ms := 0, interpolation := 1) -> void:
	Wwise.stop_event(playing_id, fade_time_ms, interpolation)

func load_bank_id(bank_id: int) -> bool:
	return Wwise.load_bank_id(bank_id)

func unload_bank_id(bank_id: int) -> bool:
	return Wwise.unload_bank_id(bank_id)

func register_game_obj(game_object: Node, obj_name: String) -> bool:
	return Wwise.register_game_obj(game_object, obj_name)

func unregister_game_obj(game_object: Node) -> bool:
	return Wwise.unregister_game_obj(game_object)

func set_switch_id(switch_group_id: int, switch_state_id: int, game_object: Node) -> bool:
	return Wwise.set_switch_id(switch_group_id, switch_state_id, game_object)

func set_state_id(state_group: int, state: int) -> bool:
	return Wwise.set_state_id(state_group, state)

func set_rtpc_value_id(rtpc_name: int, value: float, game_object: Node) -> bool:
	return Wwise.set_rtpc_value_id(rtpc_name, value, game_object)

func get_rtpc_value_id(rtpc_name: int, game_object: Node) -> float:
	return Wwise.get_rtpc_value_id(rtpc_name, game_object)

func set_3d_position(game_object: Node, transform: Transform3D) -> bool:
	return Wwise.set_3d_position(game_object, transform)
