using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze1Generator : MonoBehaviour
{
    [SerializeField]
    PrefabDatabase prefabDB;

    [SerializeField]
    int mazeX = 20;

    [SerializeField]
    int mazeY = 20;

    [SerializeField]
    Transform mazeGroup;

    Maze1Cell[,] mazeCellMap;

    Stack<Maze1Cell> pathFindingCells = new Stack<Maze1Cell>();

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        mazeCellMap = new Maze1Cell[mazeX, mazeY];

        for (int x = 0; x < mazeX; x++)
        {
            for (int y = 0; y < mazeY; y++)
            {
                Maze1Cell cell = Instantiate(prefabDB.prefabList[0], mazeGroup).GetComponent<Maze1Cell>();
                cell.transform.position = new Vector3(cell.mazeSize * x, 0, cell.mazeSize * y);

                mazeCellMap[x, y] = cell;

                cell.Init(x, y);
            }
        }

        RecursiveBackTracking(mazeCellMap[Random.Range(0, mazeX), Random.Range(0, mazeY)]);
    }

    void RecursiveBackTracking(Maze1Cell selectedCell)
    {
        selectedCell.isVisited = true;

        List<Maze1Cell> neighborUnvisitedCells = new List<Maze1Cell>();

        // Check left
        if (selectedCell.locX - 1 >= 0)
        {
            Maze1Cell checkingNeighborCell = mazeCellMap[selectedCell.locX - 1, selectedCell.locY];

            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
            }
        }

        // Check right
        if (selectedCell.locX + 1 < mazeX)
        {
            Maze1Cell checkingNeighborCell = mazeCellMap[selectedCell.locX + 1, selectedCell.locY];

            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
            }
        }

        // Check down
        if (selectedCell.locY - 1 >= 0)
        {
            Maze1Cell checkingNeighborCell = mazeCellMap[selectedCell.locX, selectedCell.locY - 1];

            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
            }
        }

        // Check up
        if (selectedCell.locY + 1 < mazeY)
        {
            Maze1Cell checkingNeighborCell = mazeCellMap[selectedCell.locX, selectedCell.locY + 1];

            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
            }
        }

        if (neighborUnvisitedCells.Count > 0)
        {
            Maze1Cell nextSelectedCell = neighborUnvisitedCells[Random.Range(0, neighborUnvisitedCells.Count)];

            if (nextSelectedCell.locX < selectedCell.locX)
            {
                nextSelectedCell.walls[0].SetActive(false);
                selectedCell.walls[1].SetActive(false);
            }
            else if (nextSelectedCell.locX > selectedCell.locX)
            {
                nextSelectedCell.walls[1].SetActive(false);
                selectedCell.walls[0].SetActive(false);
            }
            else if (nextSelectedCell.locY < selectedCell.locY)
            {
                nextSelectedCell.walls[3].SetActive(false);
                selectedCell.walls[2].SetActive(false);
            }
            else if (nextSelectedCell.locY > selectedCell.locY)
            {
                nextSelectedCell.walls[2].SetActive(false);
                selectedCell.walls[3].SetActive(false);
            }

            pathFindingCells.Push(selectedCell);
            RecursiveBackTracking(nextSelectedCell);
        }
        else if (pathFindingCells.Count > 0)
        {
            // As long as there is cell inthe stack
            // Pop up the last one
            // Roll back to last cell along the path

            Maze1Cell nextSelectedCell = pathFindingCells.Pop();
            RecursiveBackTracking(nextSelectedCell);
        }
        else
        {
            Debug.Log("Generation Done");
        }
    }
}
