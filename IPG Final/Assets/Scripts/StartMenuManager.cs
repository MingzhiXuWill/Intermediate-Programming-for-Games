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

    PlayerData[] saveLists;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
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

        int count = 0;
        foreach (FileInfo f in info)
        {
            if (f.FullName.Contains(".txt"))
            {
                string saveString = File.ReadAllText(f.FullName);
                saveLists[count] = JsonUtility.FromJson<PlayerData>(saveString);

                print("Load Save: " + saveLists[count].name);
                CreateSave(saveLists[count]);
            }
        }
    }

    public void CreateSave(PlayerData data)
    {
        GameObject save = Instantiate(save_Template, save_Template.transform.position, Quaternion.identity, save_Template.transform.parent);

        save.name = "Save " + data.name;

        save.SetActive(true);

        save.transform.Find("Icon").GetComponent<Image>().sprite = playerSets.createIcon(data.player_sprite);
        save.transform.Find("Char Name").GetComponent<TextMeshProUGUI>().text = data.name;
        save.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Level " + data.level;
        save.transform.Find("Act Number").GetComponent<TextMeshProUGUI>().text = "Act " + data.act_number.ToString();
    }
}
