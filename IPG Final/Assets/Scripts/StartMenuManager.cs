using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class StartMenuManager : MonoBehaviour
{
    public static StartMenuManager instance;

    [Header("System-----------------")]

    [SerializeField]
    BackgroundSets backgroundSets;

    [SerializeField]
    NPCSets NPCSets;

    [SerializeField]
    PlayerSets playerSets;

    [Header("GUI-----------------")]

    [SerializeField]
    GameObject panel_Save;

    [SerializeField]
    GameObject panel_Start;

    [SerializeField]
    GameObject panel_Create;

    [SerializeField]
    GameObject save_Template;

    [SerializeField]
    GameObject char_Template;

    [SerializeField]
    TMP_InputField input_Name;

    Button[] buttons_save;

    Button[] buttons_char;

    Outline[] outlines_char;

    PlayerData[] saveLists;

    PlayerDataPort playerDataPort;

    int currentSelectCharacter = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        playerDataPort = GameObject.FindGameObjectWithTag("PlayerDataPort").GetComponent<PlayerDataPort>();

        GotoStartPanel();

        LoadSaves();
        CreateCharacter();
    }

    public void GotoSavePanel()
    {
        OpenSavePanel();
        CloseStartPanel();
        CloseCreatePanel();
    }

    public void GotoStartPanel()
    {
        OpenStartPanel();
        CloseSavePanel();
        CloseCreatePanel();
    }

    public void GotoCharCreatePanel()
    {
        OpenCreatePanel();
        CloseSavePanel();
        CloseStartPanel();
    }

    public void StartAdventure()
    {
        PortNewData(input_Name.text, currentSelectCharacter);
        SceneManager.LoadScene("GameScene");
    }

    public void LoadAdventure()
    {
        SceneManager.LoadScene("GameScene");
    }

    void OpenSavePanel()
    {
        panel_Save.SetActive(true);
    }
    void CloseSavePanel()
    {
        panel_Save.SetActive(false);
    }
    void OpenStartPanel()
    {
        panel_Start.SetActive(true);
    }
    void CloseStartPanel()
    {
        panel_Start.SetActive(false);
    }
    void OpenCreatePanel()
    {
        panel_Create.SetActive(true);
    }
    void CloseCreatePanel()
    {
        panel_Create.SetActive(false);
    }

    public void CreateCharacter()
    {
        buttons_char = new Button[playerSets.Sets.Length];
        outlines_char = new Outline[playerSets.Sets.Length];

        for (int count = 0; count < buttons_char.Length; count ++)
        {
            GameObject character = Instantiate(char_Template, char_Template.transform.position, Quaternion.identity, char_Template.transform.parent);
            buttons_char[count] = character.GetComponent<Button>();

            character.SetActive(true);

            character.transform.Find("Sprite").GetComponent<Image>().sprite = playerSets.createSprite(count);
            outlines_char[count] = character.transform.Find("Select").GetComponent<Outline>();
        }

        SelectCharacter();
    }

    public void SelectCharacter()
    {
        for (int count = 0; count < outlines_char.Length; count ++)
        {
            outlines_char[count].enabled = false;
        }

        outlines_char[currentSelectCharacter].enabled = true;
    }

    public void SetSelectCharacter(Button button)
    {
        for (int count = 0; count < buttons_char.Length; count++)
        {
            if (buttons_char[count] == button)
            {
                currentSelectCharacter = count;
                break;
            }
        }

        SelectCharacter();
    }

    public void LoadSaves()
    {
        DirectoryInfo dir = new DirectoryInfo("Assets/Saves");
        FileInfo[] info = dir.GetFiles("*.txt");

        saveLists = new PlayerData[info.Length];
        buttons_save = new Button[info.Length];

        int count = 0;
        foreach (FileInfo f in info)
        {
            print(count);
            if (f.FullName.Contains(".txt"))
            {
                string saveString = File.ReadAllText(f.FullName);
                saveLists[count] = JsonUtility.FromJson<PlayerData>(saveString);

                print("Load Save: " + saveLists[count].name);
                CreateSave(saveLists[count], count);
                count++;
            }
        }
    }

    public void CreateSave(PlayerData data, int count)
    {
        print(count);
        GameObject save = Instantiate(save_Template, save_Template.transform.position, Quaternion.identity, save_Template.transform.parent);
        buttons_save[count] = save.GetComponent<Button>();

        save.name = "Save " + data.name;

        save.SetActive(true);

        save.transform.Find("Icon").GetComponent<Image>().sprite = playerSets.createIcon(data.player_sprite);
        save.transform.Find("Char Name").GetComponent<TextMeshProUGUI>().text = data.name;
        save.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Level " + data.level;
        save.transform.Find("Act Number").GetComponent<TextMeshProUGUI>().text = "Act " + IntToRoman(data.act_number);
    }

    public void PortSaveData(Button button)
    {
        int order = -1;

        for (int count = 0; count < buttons_save.Length; count ++)
        {
            if (buttons_save[count] == button)
            {
                order = count;
                break;
            }
        }
        playerDataPort.ReceiveData(saveLists[order]);
    }

    public void PortNewData(string player_name, int player_sprite)
    {
        PlayerData data = new PlayerData();

        if (string.IsNullOrWhiteSpace(player_name))
        {
            data.name = "Nameless";
        }
        else
        {
            data.name = player_name;
        }
        data.level = 1;
        data.experience = 0;
        data.act_number = 1;
        data.lootCount = 0;
        data.player_sprite = player_sprite;

        for (int count = 0; count < data.items_level.Length; count ++) {
            data.items_name[count] = "";
        }

        playerDataPort.ReceiveData(data);
    }

    public string IntToRoman(int value)
    {
        int[] arabic = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
        string[] roman = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

        string result = " ";

        for (int count = 0; count < 13; count++)
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
