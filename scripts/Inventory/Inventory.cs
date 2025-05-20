using System;
using System.Collections.Generic;
using Godot;

public class Inventory : Node, ISavable<SaveData>
{
    public Dictionary<int, ItemData> itemDatas = new Dictionary<int, ItemData>();
    public override void _EnterTree()
    {
        base._EnterTree();
        InventoryEvents.OnUpdateRequest += InventoryChanged;
        InventoryEvents.OnItemCollect += AddData;
        InventoryEvents.OnItemConsume += RemoveData;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        InventoryEvents.OnUpdateRequest -= InventoryChanged;
        InventoryEvents.OnItemCollect -= AddData;
        InventoryEvents.OnItemConsume -= RemoveData;
    }
    private void AddData(ItemData data)
    {
        GD.Print("Item collected to inventory: " + data.Name);
        itemDatas.Add(data.Id, data);
        InventoryChanged();
    }
    private void RemoveData(ItemData data)
    {
        if (itemDatas.ContainsKey(data.Id))
        {
            itemDatas.Remove(data.Id);
            InventoryChanged();
        }
        else
        {
            GD.PrintErr("Tried to remove an item that doesn't exist");
        }
    }
    private void InventoryChanged()
    {
        InventoryEvents.OnInventoryChange.Invoke(itemDatas);
    }

    public void OnSave(SaveData data)
    {
        data.collectibleData.ItemIds.Clear();
        foreach (var item in itemDatas)
        {
            data.collectibleData.ItemIds.Add(item.Value.Id);
        }
    }

    public void OnLoad(SaveData data)
    {
        itemDatas.Clear();
        foreach (var id in data.collectibleData.ItemIds)
        {
            var itemData = ResourceDatabase.ItemDatas[id];
            itemDatas.Add(id, itemData);
            itemData.IsCollected = true;
        }
    }
}