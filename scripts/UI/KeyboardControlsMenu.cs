
public class KeyboardControlsMenu : BaseControlsMenu
{
	public override void SwapKeys(InputBinding inputBinding1, InputBinding inputBinding2)
	{
		base.SwapKeys(inputBinding1, inputBinding2);
		InputRebinder.SwapKeyboardBindings(inputBinding1.actionName, inputBinding2.actionName);
	}

}
