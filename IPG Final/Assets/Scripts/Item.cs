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
}
