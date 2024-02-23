using System;

[Serializable]
public class ItemSavableData
{
    public string ItemName;
    public bool IsEquiped;
    public int ItemIndex;

    public ItemSavableData(Item item)
    {
        ItemName = item.EquipItemData.Key;
        IsEquiped = item.IsEquiped;
        ItemIndex = item.ItemIndex;
    }
}
