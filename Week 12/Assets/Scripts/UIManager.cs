using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Animator anim_PickUpItem;
    [SerializeField]
    Image image_PickUpItem;
    [SerializeField]
    Sprite[] sp_PickUpItem;


    [SerializeField]
    Color[] color_Password;
    [SerializeField]
    Sprite[] sp_Password;
    [SerializeField]
    Image bg_Password;
    [SerializeField]
    TextMeshProUGUI tX_PasswordContent;

    [SerializeField]
    TextMeshProUGUI tX_RelicContent;

    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        DisplayRelicNum();
        DisplayPasswordEnd();
    }

    public void PickUpItem(int itemID)
    {
        image_PickUpItem.sprite = sp_PickUpItem[itemID];
        anim_PickUpItem.SetTrigger("Play");

        GameManager.instance.itemCollected++;
        DisplayRelicNum();
    }

    void DisplayRelicNum()
    {
        tX_RelicContent.text = GameManager.instance.itemCollected + "/" + GameManager.instance.itemMaximumNum;
    }

    public void DisplayPassword(string password)
    {
        bg_Password.sprite = sp_Password[1];
        tX_PasswordContent.color = color_Password[1];
        tX_PasswordContent.fontSize = 20f;
        tX_PasswordContent.text = password;
    }

    public void DisplayPasswordEnd()
    {
        bg_Password.sprite = sp_Password[0];
        tX_PasswordContent.color = color_Password[0];
        tX_PasswordContent.fontSize = 12f;
        tX_PasswordContent.text = "Password";
    }
}
