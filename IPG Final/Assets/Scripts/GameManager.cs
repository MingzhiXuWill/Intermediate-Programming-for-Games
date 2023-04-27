using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ChatGPTWrapper;
//using System;

public class GameManager : MonoBehaviour
{

    [Header("Singleton")] //---------------

    public static GameManager instance;

    [Header("System")] //---------------

    [HideInInspector]
    public string[] ItemSlotName = { "Weapon", "Head", "Shoulders", "Shoulders", "Chest", "Wrist", "Hands", "Waist", "Legs", "Feet"};

    [SerializeField]
    ChatGPTCore chatGPTCore;

    [Header("GUI")] //---------------

    [SerializeField]
    TextMeshProUGUI[] text_Items;

    [SerializeField]
    Color32[] rarityColor;

    [Header("Text")] //---------------

    [SerializeField]
    [TextArea(1, 100)]
    string text_01;

    [SerializeField]
    [TextArea(1, 100)]
    string text_02;

    [Header("Player")] //---------------

    int player_Level = 1;
    Item[] player_Items;
    float player_Experience = 0;



    void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
        }

        chatGPTCore.Init();
    }

    private void Start()
    {
        //print(text_02);
        CreateRandomItem();
    }

    public void CreateRandomItem()
    {
        string prompt = "Provide one random name for one ";
        prompt += ItemSlotName[Random.Range(0, ItemSlotName.Length)];
        prompt += text_01;

        chatGPTCore.SendToChatGPT(prompt);
    }

    public void ReceiveChatGPTReply(string message)
    {
        print(message);

        ItemJsonReceiver itemJSON = JsonUtility.FromJson<ItemJsonReceiver>(message);
        text_Items[1].text = itemJSON.item_name;

        //ActJsonReceiver actJSON = JsonUtility.FromJson<ActJsonReceiver>(message);
    }
}
