extends ParleyActionInterface

func run(_ctx: ParleyContext, values: Array) -> int:
	ParleyActions.ChangeMode(values)
	return OK
