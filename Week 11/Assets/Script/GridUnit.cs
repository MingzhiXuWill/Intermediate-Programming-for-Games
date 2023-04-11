using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridUnit : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer highlight;
    [SerializeField]
    Color[] highLightColor;

    [SerializeField]
    public int x;
    [SerializeField]
    public int y;
    [HideInInspector]
    public int tileType;

    [SerializeField]
    TextMeshPro tx_Info;

    [SerializeField]
    GameObject pathfindingIndicator;

    public void Init(int posX, int posY, int tT)
    {
        x = posX;
        y = posY;
        tileType = tT;

        //tx_Info.text = "X:" + x + "\nY:" + y;
    }

    private void OnMouseEnter()
    {
        highlight.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.gameObject.SetActive(false);
    }

    public void DisplayReachableGrid()
    {
        highlight.color = highLightColor[3];
        highlight.gameObject.SetActive(true);
    }
    public void ResetGrid()
    {

        highlight.gameObject.SetActive(false);
        highlight.color = highLightColor[tileType];
    }

    public void DisplayPathfinding(string displayContent)
    {
        pathfindingIndicator.SetActive(true);
    }

    public void HidePathfinding()
    {
        pathfindingIndicator.SetActive(false);
    }
}
