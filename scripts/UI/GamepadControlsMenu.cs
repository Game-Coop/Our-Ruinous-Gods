public class GamepadControlsMenu : BaseControlsMenu
{
	public override void SwapKeys(InputBindingButton inputBinding1, InputBindingButton inputBinding2)
	{
		base.SwapKeys(inputBinding1, inputBinding2);
		var inputName1 = inputBinding1.keyInfo != null ? inputBinding1.keyInfo.InputName : "";
		var inputName2 = inputBinding2.keyInfo != null ? inputBinding2.keyInfo.InputName : "";
		InputBindings.SwapGamepadKeys(inputName1, inputName2);
	}
}
