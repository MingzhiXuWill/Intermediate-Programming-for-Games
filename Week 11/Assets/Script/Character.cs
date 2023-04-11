using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer highLight;
    [HideInInspector]
    public GridUnit currentLocation;

    public int actionPoints = 10;

    private void OnMouseUp()
    {
        print("start");
        if (!GridManager.instance.inMovingMode)
        {
            //Start movement
            GridManager.instance.StartMovingMode(); 
        }
        else
        {
            //Stop movement
            GridManager.instance.StopMovingMode();
        }
    }
    private void OnMouseEnter()
    {
        highLight.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        highLight.gameObject.SetActive(false);
    }
}
