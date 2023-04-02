using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEmotionController : MonoBehaviour
{
    [SerializeField]
    Sprite joyful;
    [SerializeField]
    Sprite serious;
    [SerializeField]
    Sprite delightful;
    [SerializeField]
    Sprite surprise;
    [SerializeField]
    Sprite laugh;

    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetCharacterEmotion(string animID)
    {
        Debug.Log("change emotion to: " + animID);

        if (animID == "joyful")
        {
            GetComponent<Image>().sprite = joyful;
        }
        else if (animID == "serious")
        {
            GetComponent<Image>().sprite = serious;
        }
        else if (animID == "delightful")
        {
            GetComponent<Image>().sprite = delightful;
        }
        else if (animID == "surprise")
        {
            GetComponent<Image>().sprite = surprise;
        }
        else if (animID == "laugh")
        {
            GetComponent<Image>().sprite = laugh;
        }
    }
}
