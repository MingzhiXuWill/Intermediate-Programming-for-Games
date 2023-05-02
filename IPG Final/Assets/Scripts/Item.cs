using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string itemName;

    public int itemLevel;
    public int rarity;

    public int slotNumber;

    public void ReceiveInfoFromJson(ItemJsonReceiver json)
    {
        itemName = json.item_name;
    }

    public void ReceiveInfoFromPort(string name, int level, int rarity, int slotNumber)
    {
        this.itemName = name;
        this.itemLevel = level;
        this.rarity = rarity;
        this.slotNumber = slotNumber;
    }
}
