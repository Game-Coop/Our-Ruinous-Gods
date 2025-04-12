public class GamepadControlsMenu : BaseControlsMenu
{
	public override void SwapKeys(InputBindingButton inputBinding1, InputBindingButton inputBinding2)
	{
		base.SwapKeys(inputBinding1, inputBinding2);
		InputRebinder.SwapGamepadBindings(inputBinding1.actionName, inputBinding2.actionName);
	}
}
