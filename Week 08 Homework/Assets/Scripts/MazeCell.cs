using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [HideInInspector]
    public bool isVisited = false;

    // Unity size for the cell
    public float mazeSize = 5;

    public GameObject[] walls;

    // Actual index in the int[,] of or random generated map
    [HideInInspector]
    public int locX;
    [HideInInspector]
    public int locY;

    public void Init(int x, int y)
    {
        locX = x;
        locY = y;
    }
}
