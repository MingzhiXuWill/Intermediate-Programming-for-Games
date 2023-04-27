using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    string itemName;

    int itemLevel;
    int rarity;

    int slotNumber;

    public void ReceiveInfoFromJson(ItemJsonReceiver json)
    {
        itemName = json.item_name;
    }
}
