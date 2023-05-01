using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ChatGPTWrapper;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    enum PlayerState { Rest, Adventure};
    PlayerState myPlayerState = PlayerState.Rest;

    public static GameManager instance;

    [Header("System-----------------")] 

    string[] ItemSlotName = { "Weapon", "Head", "Neck", "Shoulders", "Cloak", "Chest", "Wrist", "Hands", "Waist", "Legs", "Feet", "Ring", "Trinket"};

    string[] RarityName = { "Poor", "Common", "Uncommon", "Rare", "Epic", "Legendary"};

   [SerializeField]
    ChatGPTCore chatGPTCore;

    Item currentItem = new Item();
    Act currentAct = new Act();

    [SerializeField]
    float eventTimerMax;
    float eventTimer = 0;

    [SerializeField]
    int eventNumberMax;

    [SerializeField]
    float experienceMultiplier = 2;

    int currentSlotNumber = -1;

    [SerializeField]
    BackgroundSets backgroundSets;

    [SerializeField]
    NPCSets NPCSets;

    [SerializeField]
    PlayerSets playerSets;

    [SerializeField]
    int lootCountMax;

    float lootTextCount = 0;

    [SerializeField]
    float lootTextCountMax;

    [Header("GUI-----------------")]

    [SerializeField]
    TextMeshProUGUI[] text_Items;

    [SerializeField]
    Color32[] rarityColor;

    [SerializeField]
    TextMeshProUGUI text_PlayerName;

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
    GameObject GUI_Rest;

    [SerializeField]
    Image image_Player_Sprite;

    [SerializeField]
    Image image_Player_Shadow;

    [SerializeField]
    Image image_NPC_Sprite;

    [SerializeField]
    Image image_NPC_Shadow;

    [SerializeField]
    Image image_Background;

    [SerializeField]
    TextMeshProUGUI text_Loot;

    [Header("Prompt-----------------")]

    [SerializeField]
    [TextArea(1, 100)]
    string prompt_item;

    [SerializeField]
    string prompt_item_replace;

    [SerializeField]
    string prompt_rarity_replace;

    [SerializeField]
    [TextArea(1, 100)]
    string prompt_place;

    [SerializeField]
    string prompt_NPC_list_replace;

    [SerializeField]
    string prompt_place_list_replace;

    [SerializeField]
    string prompt_event_number_replace;

    [Header("Player-----------------")]

    string player_Name = "No one";
    int player_Level = 1;
    float player_Experience = 0;
    float player_Experience_Current = 0;

    int player_sprite = 0;

    int actNumber = 1;
    int eventNumber = 0;

    int lootCount = 0;

    Item[] player_Items = new Item[13];

    PlayerDataPort playerDataPort;

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
        PlayerDataPort playerDataPort = transform.parent.Find("PlayerDataPort").GetComponent<PlayerDataPort>();

        // Setup item list
        for (int count = 0; count < player_Items.Length; count ++) 
        {
            player_Items[count] = new Item();
        }

        // Temp setup player
        SetPlayerSprite(player_sprite);

        // Setup player name
        SetPlayerName();

        // Start Act
        GenerateAct();
    }

    private void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            SavePlayerData();
        }
        if (Input.GetKeyDown("u"))
        {
            GenerateItem();
        }

        // Event Timer
        if (myPlayerState == PlayerState.Adventure)
        {
            if (eventTimer > eventTimerMax)
            {
                if (eventNumber < eventNumberMax - 1)
                {
                    //print(lootCount);
                    eventTimer = 0;
                    eventNumber++;
                    lootCount++;
                    if (lootCount >= lootCountMax)
                    {
                        lootCount = 0;
                        GenerateItem();
                    }

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
        
        // UI bar
        SetEventBar();
        SetExpBar();
        LootTextUpdate();
    }

    public void GenerateItem()
    {
        currentSlotNumber = Random.Range(0, ItemSlotName.Length - 1);

        Item tempItem = new Item();

        tempItem.slotNumber = currentSlotNumber;
        tempItem.itemLevel = player_Level;

        int chance = Random.Range(0, 100); // Rarity chance

        if (chance > 95) // Legendary
        {
            tempItem.rarity = 5;
        }
        else if (chance > 85) // Epic 
        {
            tempItem.rarity = 4;
        }
        else if (chance > 70) // Rare
        {
            tempItem.rarity = 3;
        }
        else if(chance > 50) // Uncommon 
        {
            tempItem.rarity = 2;
        }
        else if(chance > 15) // Common 
        {
            tempItem.rarity = 1;
        }
        else // Poor 
        {
            tempItem.rarity = 0;
        }

        if (CompareItem(tempItem))
        {
            string prompt = prompt_item;

            prompt = prompt.Replace(prompt_item_replace, ItemSlotName[currentSlotNumber]);

            prompt = prompt.Replace(prompt_rarity_replace, RarityName[tempItem.rarity]);

            //print(currentSlotNumber);
            //print(prompt);

            player_Items[currentSlotNumber].slotNumber = currentSlotNumber;
            player_Items[currentSlotNumber].itemLevel = player_Level;
            player_Items[currentSlotNumber].rarity = tempItem.rarity;

            chatGPTCore.SendToChatGPT(prompt);
        }

        //print(tempItem.slotNumber);
    }

    public void GenerateAct()
    {
        string prompt = prompt_place;

        prompt = prompt.Replace(prompt_NPC_list_replace, NPCSets.createNameList());

        prompt = prompt.Replace(prompt_place_list_replace, backgroundSets.createNameList());

        prompt = prompt.Replace(prompt_event_number_replace, eventNumberMax.ToString());

        print(prompt);

        ResetEvent();
        ResetAct();

        SetNPCSprite(null);

        SetBackgroundSprite("Camp");

        myPlayerState = PlayerState.Rest;
        GUI_Rest.SetActive(true);
        eventNumber = 0;

        chatGPTCore.SendToChatGPT(prompt);
    }

    public void ReceiveChatGPTReply(string message)
    {
        print("Received: " + message);

        if (message.Contains("item_name"))
        {
            ItemJsonReceiver itemJSON = JsonUtility.FromJson<ItemJsonReceiver>(message);
            currentItem.ReceiveInfoFromJson(itemJSON);

            player_Items[currentSlotNumber].itemName = itemJSON.item_name;

            SetLootText(player_Items[currentSlotNumber]);

            EquipItem(player_Items[currentSlotNumber]);
        }
        else if (message.Contains("place_name"))
        {
            ActJsonReceiver actJSON = JsonUtility.FromJson<ActJsonReceiver>(message);
            currentAct.ReceiveInfoFromJson(actJSON);

            myPlayerState = PlayerState.Adventure;
            GUI_Rest.SetActive(false);

            SetAct();
            SetEvent();
        }

        chatGPTCore.ResetChat();
    }

    void EquipItem(Item item)
    {
        text_Items[currentSlotNumber].text = item.itemName + " +" + item.itemLevel;
        text_Items[currentSlotNumber].color = rarityColor[item.rarity];
    }

    bool CompareItem(Item item)
    {
        int oldScore = player_Items[item.slotNumber].itemLevel + player_Items[item.slotNumber].rarity * 2;
        int newScore = item.itemLevel + item.rarity * 2;

        if (oldScore < newScore)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SetExpBar()
    {
        player_Experience_Current = Mathf.Lerp(player_Experience_Current, player_Experience, 0.01f);
        slider_ExpBar.value = player_Experience_Current / player_Level / experienceMultiplier;
    }

    void SetEventBar()
    {
        slider_EventBar.value = eventTimer / eventTimerMax;
    }

    void SetPlayerName()
    {
        text_PlayerName.text = player_Name;
    }

    void SetEvent()
    {
        text_Event.text = currentAct.events[eventNumber];
        SetNPCSprite(currentAct.NPCs[eventNumber]);
    }

    void SetNPCSprite(string NPCName)
    {
        Sprite sprite = NPCSets.createSprite(NPCName);

        if (sprite != null)
        {
            image_NPC_Shadow.gameObject.SetActive(true);
            image_NPC_Sprite.gameObject.SetActive(true);

            image_NPC_Shadow.sprite = sprite;
            image_NPC_Sprite.sprite = sprite;
        }
        else
        {
            image_NPC_Shadow.gameObject.SetActive(false);
            image_NPC_Sprite.gameObject.SetActive(false);
        }
    }

    void SetPlayerSprite(int playerNumber)
    {
        Sprite sprite = playerSets.createSprite(playerNumber);

        if (sprite != null)
        {
            image_Player_Shadow.gameObject.SetActive(true);
            image_Player_Sprite.gameObject.SetActive(true);

            image_Player_Shadow.sprite = sprite;
            image_Player_Sprite.sprite = sprite;
        }
        else
        {
            image_Player_Shadow.gameObject.SetActive(false);
            image_Player_Sprite.gameObject.SetActive(false);
        }
    }

    void SetBackgroundSprite(string BackgroundName)
    {
        Sprite sprite = backgroundSets.createSprite(BackgroundName);

        if (sprite != null)
        {
            image_Background.gameObject.SetActive(true);

            image_Background.sprite = sprite;
        }
        else
        {
            image_Background.gameObject.SetActive(false);
        }
    }

    void SetAct()
    {
        text_ActName.text = "Act" + IntToRoman(actNumber) + " - " + currentAct.placeName;
        text_Act.text = currentAct.placeDescription;

        SetBackgroundSprite(currentAct.environment);
    }

    void ResetEvent()
    {
        text_Event.text = "";
    }

    void ResetAct()
    {
        text_ActName.text = "";
        text_Act.text = "";
    }

    void SetLevel()
    {
        text_Level.text = "Level " + player_Level;
    }

    void GainExp()
    {
        player_Experience++;

        if (player_Experience >= player_Level * experienceMultiplier)
        {
            player_Experience = 0;

            player_Level++;

            SetLevel();
        }
    }

    void SetLootText(Item item)
    {
        string Color = ColorUtility.ToHtmlStringRGBA(rarityColor[item.rarity]);

        text_Loot.text = "You found " + "<color=#" + Color + ">" + item.itemName + " +" + item.itemLevel + "</color>";

        text_Loot.gameObject.SetActive(true);

        lootTextCount = lootTextCountMax;
    }

    void LootTextUpdate()
    {
        if (lootTextCount > 0)
        {
            lootTextCount -= Time.deltaTime;
        }
        else
        {
            lootTextCount = 0;

            text_Loot.gameObject.SetActive(false);
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

    public void SavePlayerData()
    {
        PlayerData playerData = new PlayerData
        {
            name = player_Name,
            level = player_Level,
            experience = (int)player_Experience_Current,
            act_number = actNumber,
            player_sprite = player_sprite,
        };

        for (int count = 0; count < player_Items.Length; count ++)
        {
            playerData.items_level[count] = player_Items[count].itemLevel;
            playerData.items_name[count] = player_Items[count].itemName;
            playerData.items_rarity[count] = player_Items[count].rarity;
        }

        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(Application.dataPath + "/Saves/" + player_Name + ".txt", json);

        print("Saved: " + player_Name);
    }
}

public class PlayerData
{
    public string name;
    public int level;
    public int[] items_level = new int[13];
    public int[] items_rarity = new int[13];
    public string[] items_name = new string[13];
    public int experience;
    public int act_number;
    public int player_sprite;
}
