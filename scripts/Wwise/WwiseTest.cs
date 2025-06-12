
using Godot;

public partial class WwiseTest : Node
{
	double progress = 0f;
	public override void _Ready()
	{
		base._Ready();
		WwiseManager.LoadBank(AK.BANKS.TEST_BANK.Name);
		WwiseManager.PostEvent(AK.EVENTS.PLAY_TEST.Id, this);
	}
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed("inventory_toggle"))
		{
			// WwiseManager.SetSwitch(AK.SWITCHES.TESTING_SWITCH_GROUP.GROUP, AK.SWITCHES.TESTING_SWITCH_GROUP.SWITCH.TESTING_SWITCH_ON, this);
			WwiseManager.SetState(AK.STATES.TESTING_STATE.GROUP, AK.STATES.TESTING_STATE.STATE.TESTING_STATE_ON);
			WwiseManager.PostEvent(AK.EVENTS.PLAY_TEST.Id, this);
		}
		else if (@event.IsActionPressed("journal_toggle"))
		{
			// WwiseManager.SetSwitch(AK.SWITCHES.TESTING_SWITCH_GROUP.GROUP, AK.SWITCHES.TESTING_SWITCH_GROUP.SWITCH.TESTING_SWITCH_OFF, this);
			WwiseManager.SetState(AK.STATES.TESTING_STATE.GROUP, AK.STATES.TESTING_STATE.STATE.TESTING_STATE_OFF);
			WwiseManager.PostEvent(AK.EVENTS.PLAY_TEST.Id, this);
		}
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
		progress += delta;
		progress %= 1;
		WwiseManager.SetRTPCValue(AK.RTPCS.TESTING_RTIPC.Id, (float)progress, this);
	}

}
