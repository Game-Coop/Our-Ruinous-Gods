// Auto-generated from Wwise SoundBanksInfo.xml
namespace AK
{
	public static class BANKS
	{
        public static readonly (string Name, uint Id) INIT = ("Init", 1355168291U);
        public static readonly (string Name, uint Id) TEST_BANK = ("Test_Bank", 2314467862U);
	}
	public static class EVENTS
	{
        public static readonly (string Name, uint Id) PLAY_TEST = ("Play_Test", 3187507146U);
	}
	public static class BUSSES
	{
        public static readonly (string Name, uint Id) MASTER_AUDIO_BUS = ("Master Audio Bus", 3803692087U);
	}
	public static class RTPCS
	{
        public static readonly (string Name, uint Id) TESTING_RTIPC = ("Testing_RTIPC", 686920922U);
	}
	public static class AUDIO_DEVICES
	{
        public static readonly (string Name, uint Id) NO_OUTPUT = ("No_Output", 2317455096U);
        public static readonly (string Name, uint Id) SYSTEM = ("System", 3859886410U);
	}
	public static class SWITCHES
	{
		public static class TESTING_SWITCH_GROUP
		{
            public const uint GROUP = 1993586446U;
			public static class SWITCH
			{
                public const uint TESTING_SWITCH_OFF = 3908775632U;
                public const uint TESTING_SWITCH_ON = 3982860346U;
			}
		}
	}
	public static class STATES
	{
		public static class TESTING_STATE
		{
            public const uint GROUP = 1146584329U;
			public static class STATE
			{
                public const uint NONE = 748895195U;
                public const uint TESTING_STATE_OFF = 2680282971U;
                public const uint TESTING_STATE_ON = 3822738919U;
			}
		}
	}
} // namespace AK
