
using System;
using System.Collections.Generic;

public static class InventoryEvents
{
    public static Action OnInventoryOpen;
    public static Action OnInventoryClose;
    public static Action<ItemData> OnItemCollect;
    public static Action<ItemData> OnItemInspect;
    public static Action<ItemData> OnItemConsume;
    public static Action<Dictionary<int, ItemData>> OnInventoryChange;
    public static Action<ItemData, ItemData> OnItemsCombine;
    public static Action OnUpdateRequest;
}