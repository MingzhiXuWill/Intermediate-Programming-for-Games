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

    Button[] buttons_save;
    
    PlayerData[] saveLists;

    PlayerDataPort playerDataPort;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        PlayerDataPort playerDataPort = transform.parent.Find("PlayerDataPort").GetComponent<PlayerDataPort>();

        GotoStartPanel();

        LoadSaves();
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

    public void LoadSaves()
    {
        DirectoryInfo dir = new DirectoryInfo("Assets/Saves");
        FileInfo[] info = dir.GetFiles("*.txt");

        saveLists = new PlayerData[info.Length];
        buttons_save = new Button[info.Length];

        int count = 0;
        foreach (FileInfo f in info)
        {
            if (f.FullName.Contains(".txt"))
            {
                string saveString = File.ReadAllText(f.FullName);
                saveLists[count] = JsonUtility.FromJson<PlayerData>(saveString);

                print("Load Save: " + saveLists[count].name);
                CreateSave(saveLists[count], count);
            }
        }
    }

    public void CreateSave(PlayerData data, int count)
    {
        GameObject save = Instantiate(save_Template, save_Template.transform.position, Quaternion.identity, save_Template.transform.parent);
        buttons_save[count] = save.GetComponent<Button>();

        save.name = "Save " + data.name;

        save.SetActive(true);

        save.transform.Find("Icon").GetComponent<Image>().sprite = playerSets.createIcon(data.player_sprite);
        save.transform.Find("Char Name").GetComponent<TextMeshProUGUI>().text = data.name;
        save.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Level " + data.level;
        save.transform.Find("Act Number").GetComponent<TextMeshProUGUI>().text = "Act " + data.act_number.ToString();
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

        data.name = player_name;
        data.level = 1;
        data.experience = 0;
        data.act_number = 1;
        data.player_sprite = player_sprite;
    }
}
