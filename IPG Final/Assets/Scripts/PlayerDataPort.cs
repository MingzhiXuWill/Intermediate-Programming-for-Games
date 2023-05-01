using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataPort : MonoBehaviour
{
    public static PlayerDataPort instance;

    [HideInInspector]
    public string name;
    [HideInInspector]
    public int level;
    [HideInInspector]
    public int[] items_level = new int[13];
    [HideInInspector]
    public int[] items_rarity = new int[13];
    [HideInInspector]
    public string[] items_name = new string[13];
    [HideInInspector]
    public int experience;
    [HideInInspector]
    public int act_number;
    [HideInInspector]
    public int player_sprite;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ReceiveData(PlayerData data)
    {
        name = data.name;
        level = data.level;
        items_level = data.items_level;
        items_rarity = data.items_rarity;
        items_name = data.items_name;
        experience = data.experience;
        act_number = data.act_number;
        player_sprite = data.player_sprite;
    }

    public void ResetData()
    {

    }
}
