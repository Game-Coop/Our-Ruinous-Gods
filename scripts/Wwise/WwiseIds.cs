// Auto-generated from Wwise SoundBanksInfo.xml
namespace AK
{

    public static class EVENTS
    {
        public static readonly (string Name, uint Id) PLAY_TEST = ("Play_Test", 3187507146u);
    }

    public static class BANKS
    {
        public static readonly (string Name, uint Id) INIT = ("Init", 1355168291u);
        public static readonly (string Name, uint Id) TEST_BANK = ("Test_Bank", 2314467862u);
    }

    public static class BUSSES
    {
        public static readonly (string Name, uint Id) MASTER_AUDIO_BUS = ("Master Audio Bus", 3803692087u);
    }

    public static class AUDIO_DEVICES
    {
        public static readonly (string Name, uint Id) NO_OUTPUT = ("No_Output", 2317455096u);
        public static readonly (string Name, uint Id) SYSTEM = ("System", 3859886410u);
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
            } // class SWITCH
        } // class TESTING_SWITCH_GROUP

    } // class SWITCHES

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
            } // class STATE
        } // class TESTING_STATE

    } // class STATES

    public static class RTPCS
    {
        public static readonly (string Name, uint Id) TESTING_RTIPC = ("Testing_RTIPC", 686920922u);
    }
} // namespace AK
