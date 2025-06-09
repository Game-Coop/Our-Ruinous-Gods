
using Godot;

public partial class WwiseTest : Node
{
	public override void _Ready()
	{
		base._Ready();
		WwiseManager.LoadBank(AK.BANKS.TEST_BANK);
		// WwiseManager.PostEvent(AK.EVENTS.PLAY_TEST, this);
	}

}
