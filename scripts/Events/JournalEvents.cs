
using System;
using System.Collections.Generic;

public static class JournalEvents
{
    //example
    public static Action<JournalData> OnEntryCollect;
    public static Action OnJournalOpen;
    public static Action OnJournalClose;
    public static Action<Dictionary<int,JournalData>> OnJournalChange;
    public static Action OnUpdateRequest;
}