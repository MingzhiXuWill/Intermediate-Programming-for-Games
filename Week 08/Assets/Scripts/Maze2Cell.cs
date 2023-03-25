using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Maze2TunnelDirectionIndicator { None, Left, Right, Up, Down}

public class Maze2Cell : MonoBehaviour
{
    public Maze2TunnelDirectionIndicator tunnelDirection = Maze2TunnelDirectionIndicator.None;

    [HideInInspector]
    public bool isVisited = false;

    // Unity size for the cell
    public float mazeSize = 5;

    public GameObject wall;

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
    