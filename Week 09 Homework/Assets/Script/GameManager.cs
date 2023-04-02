using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ChatGPTWrapper;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    CharacterEmotionController characterEmotionController;

    [SerializeField]
    ChatGPTConversation chatGPT;

    [SerializeField]
    TMP_InputField iF_PlayerTalk;

    [SerializeField]
    TextMeshProUGUI tX_AIReply;

    [SerializeField]
    string npcName = "Coco";
    [SerializeField]
    string playerName = "Player";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        chatGPT.Init();
    }

    private void Start()
    {
        chatGPT.SendToChatGPT("{\"player_said\":" + "\"Hello Feater!\"}");
    }

    void Update()
    {
        if (Input.GetButtonUp("Submit"))
        {
            SubmitChatMessage();
        }
    }

    public void SubmitChatMessage()
    {
        if (iF_PlayerTalk.text != "")
        {
            chatGPT.SendToChatGPT("{\"player_said\":\"" + iF_PlayerTalk.text + "\"}");
            ClearText();
        }
    }

    void ClearText()
    {
        iF_PlayerTalk.text = "";
    }

    public void ReceiveChatGPTReply(string message)
    {
        print(message);

        try
        {
            if (!message.EndsWith("}"))
            {
                if (message.Contains("}"))
                {
                    message = message.Substring(0, message.LastIndexOf("}") + 1);
                }
                else
                {
                    message += "}";
                }
            }
            NpcJsonReceiver npcJSON = JsonUtility.FromJson<NpcJsonReceiver>(message);
            string talkLine = npcJSON.reply_to_player;

            characterEmotionController.SetCharacterEmotion(npcJSON.animation_name);

            tX_AIReply.text = "<color=#ff7082>" + npcName + ": </color>" + talkLine;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            string talkLine = "Don't say that!";
            tX_AIReply.text = "<color=#ff7082>" + npcName + ": </color>" + talkLine;
        }
    }
}
