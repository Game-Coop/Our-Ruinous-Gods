
using System;

public static class JournalEvents
{
    //example
    public static Action<JournalData> OnEntryCollect;
    public static Action OnJournalOpen;
    public static Action OnJournalClose;
}