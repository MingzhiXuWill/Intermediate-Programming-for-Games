using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ChatGPTWrapper;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum PlayerState { Rest, Adventure};
    PlayerState myPlayerState = PlayerState.Rest;

    [Header("Singleton")] //---------------

    public static GameManager instance;

    [Header("System")] //---------------

    [HideInInspector]
    public string[] ItemSlotName = { "Weapon", "Head", "Shoulders", "Shoulders", "Chest", "Wrist", "Hands", "Waist", "Legs", "Feet"};

    [SerializeField]
    ChatGPTCore chatGPTCore;

    Item currentItem = new Item();
    Act currentAct = new Act();

    [SerializeField]
    float eventTimerMax;
    float eventTimer = 0;

    [Header("GUI")] //---------------

    [SerializeField]
    TextMeshProUGUI[] text_Items;

    [SerializeField]
    TextMeshProUGUI text_ActName;

    [SerializeField]
    TextMeshProUGUI text_Act;

    [SerializeField]
    TextMeshProUGUI text_Event;

    [SerializeField]
    TextMeshProUGUI text_Level;

    [SerializeField]
    Slider slider_EventBar;

    [SerializeField]
    Slider slider_ExpBar;

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
    float player_Experience = 0;

    int actNumber = 1;
    int eventNumber = 0;

    Item[] player_Items;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        chatGPTCore.Init();
    }

    private void Start()
    {
        GenerateAct();
        GenerateItem();
    }

    private void Update()
    {
        if (myPlayerState == PlayerState.Adventure)
        {
            if (eventTimer > eventTimerMax)
            {
                if (eventNumber <= 8)
                {
                    eventTimer = 0;
                    eventNumber++;

                    GainExp();
                    SetEvent();
                }
                else
                {
                    actNumber++;

                    GenerateAct();
                }
            }
            else
            {
                eventTimer += Time.deltaTime;
            }
        }
        else
        {
            eventTimer = 0;
        }
        
        SetEventBar();
        SetExpBar();
    }

    public void GenerateItem()
    {
        string prompt = "Provide one random name for one ";
        prompt += ItemSlotName[Random.Range(0, ItemSlotName.Length)];
        prompt += text_01;

        chatGPTCore.SendToChatGPT(prompt);
    }

    public void GenerateAct()
    {
        string prompt = text_02;

        myPlayerState = PlayerState.Rest;
        eventNumber = 0;

        chatGPTCore.SendToChatGPT(prompt);
    }

    public void ReceiveChatGPTReply(string message)
    {
        print(message);

        if (message.Contains("item_name"))
        {
            ItemJsonReceiver itemJSON = JsonUtility.FromJson<ItemJsonReceiver>(message);
            currentItem.ReceiveInfoFromJson(itemJSON);

            text_Items[1].text = itemJSON.item_name;
            text_Items[1].color = rarityColor[2];
        }
        else if (message.Contains("place_name"))
        {
            ActJsonReceiver actJSON = JsonUtility.FromJson<ActJsonReceiver>(message);
            currentAct.ReceiveInfoFromJson(actJSON);

            myPlayerState = PlayerState.Adventure;

            SetAct();
            SetEvent();
        }
    }

    void SetExpBar()
    {
        slider_ExpBar.value = player_Experience / player_Level / 2;
    }

    void SetEventBar()
    {
        slider_EventBar.value = eventTimer / eventTimerMax;
    }

    void SetEvent()
    {
        text_Event.text = currentAct.events[eventNumber];
    }

    void SetAct()
    {
        text_ActName.text = "Act" + IntToRoman(actNumber) + currentAct.placeName;
        text_Act.text = currentAct.placeDescription;
    }

    void SetLevel()
    {
        text_Level.text = "Level " + player_Level;
    }

    void GainExp()
    {
        player_Experience++;

        if (player_Experience >= player_Level * 2)
        {
            player_Experience = 0;

            player_Level++;

            SetLevel();
        }
    }


    string IntToRoman(int value)
    {
        int[] arabic = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
        string[] roman = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

        string result = " ";

        for (int count = 0; count < 13; count ++)
        {
            while (value >= arabic[count])
            {
                result = result + roman[count];
                value = value - arabic[count];
            }
        }
        return result + " ";
    }
}
