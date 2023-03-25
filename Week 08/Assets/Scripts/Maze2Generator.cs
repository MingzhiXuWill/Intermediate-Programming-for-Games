using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze2Generator : MonoBehaviour
{
    [SerializeField]
    PrefabDatabase prefabDB;

    [SerializeField]
    int mazeX = 59;

    [SerializeField]
    int mazeY = 59;

    [SerializeField]
    Transform mazeGroup;

    Maze2Cell[,] mazeCellMap;

    List<Maze2Cell> unvisitedCells = new List<Maze2Cell>();

    void Start()
    {
        GenerateMaze();
    }
    void GenerateMaze()
    {
        mazeCellMap = new Maze2Cell[mazeX, mazeY];

        for (int x = 0; x < mazeX; x++)
        {
            for (int y = 0; y < mazeY; y++)
            {
                Maze2Cell cell = Instantiate(prefabDB.prefabList[1], mazeGroup).GetComponent<Maze2Cell>();
                cell.transform.position = new Vector3(cell.mazeSize * x, 0, cell.mazeSize * y);

                mazeCellMap[x, y] = cell;

                cell.Init(x, y);
            }
        }

        Maze2Cell startCell = mazeCellMap[1, 1];

        unvisitedCells.Add(startCell);
        RecursiveRandomPrim(startCell);
    }

    void RecursiveRandomPrim(Maze2Cell startCell)
    {
        unvisitedCells.Remove(startCell);

        if (!startCell.isVisited)
        {
            startCell.isVisited = true;
            startCell.wall.SetActive(false);

            if (startCell.tunnelDirection == Maze2TunnelDirectionIndicator.Right)
            {
                mazeCellMap[startCell.locX - 1, startCell.locY].wall.SetActive(false);
            }
            else if (startCell.tunnelDirection == Maze2TunnelDirectionIndicator.Left)
            {
                mazeCellMap[startCell.locX + 1, startCell.locY].wall.SetActive(false);
            }
            else if (startCell.tunnelDirection == Maze2TunnelDirectionIndicator.Up)
            {
                mazeCellMap[startCell.locX, startCell.locY + 1].wall.SetActive(false);
            }
            else if (startCell.tunnelDirection == Maze2TunnelDirectionIndicator.Down)
            {
                mazeCellMap[startCell.locX, startCell.locY - 1].wall.SetActive(false);
            }

            List<Maze2Cell> neightborUnvisitedCells = CheckCellSurroundings(startCell);

            if (neightborUnvisitedCells.Count > 0)
            {
                Maze2Cell endCell = neightborUnvisitedCells[Random.Range(0, neightborUnvisitedCells.Count)];
                endCell.isVisited = true;
                endCell.wall.SetActive(false);

                if (endCell.locX < startCell.locX)
                {
                    mazeCellMap[startCell.locX - 1, startCell.locY].wall.SetActive(false);
                }
                else if (endCell.locX > startCell.locX)
                {
                    mazeCellMap[startCell.locX + 1, startCell.locY].wall.SetActive(false);
                }
                else if (endCell.locY < startCell.locY)
                {
                    mazeCellMap[startCell.locX, startCell.locY - 1].wall.SetActive(false);
                }
                else if (endCell.locY > startCell.locY)
                {
                    mazeCellMap[startCell.locX, startCell.locY + 1].wall.SetActive(false);
                }

                neightborUnvisitedCells.Remove(endCell);

                unvisitedCells.AddRange(neightborUnvisitedCells);
                unvisitedCells.AddRange(CheckCellSurroundings(endCell));
            }
        }
        if (unvisitedCells.Count > 0)
        {
            // As long as there is a cell in the list
            // Randomly choose one cell and continus

            RecursiveRandomPrim(unvisitedCells[Random.Range(0, unvisitedCells.Count)]);
        }
        else
        {
            Debug.Log("Generation Done");
        }
    }

    List<Maze2Cell> CheckCellSurroundings(Maze2Cell cell)
    {
        List<Maze2Cell> neighborUnvisitedCells = new List<Maze2Cell>();
        if (cell.locX - 2 > 0)
        {
            Maze2Cell checkingNeighborCell = mazeCellMap[cell.locX - 2, cell.locY];
            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
                checkingNeighborCell.tunnelDirection = Maze2TunnelDirectionIndicator.Left;
            }
        }
        if (cell.locX + 2 < mazeX - 1)
        {
            Maze2Cell checkingNeighborCell = mazeCellMap[cell.locX + 2, cell.locY];
            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
                checkingNeighborCell.tunnelDirection = Maze2TunnelDirectionIndicator.Right;
            }
        }
        if (cell.locY - 2 > 0)
        {
            Maze2Cell checkingNeighborCell = mazeCellMap[cell.locX, cell.locY - 2];
            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
                checkingNeighborCell.tunnelDirection = Maze2TunnelDirectionIndicator.Up;
            }
        }
        if (cell.locY + 2 < mazeY - 1)
        {
            Maze2Cell checkingNeighborCell = mazeCellMap[cell.locX, cell.locY + 2];
            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
                checkingNeighborCell.tunnelDirection = Maze2TunnelDirectionIndicator.Down;
            }
        }

        return neighborUnvisitedCells;
    }
}
