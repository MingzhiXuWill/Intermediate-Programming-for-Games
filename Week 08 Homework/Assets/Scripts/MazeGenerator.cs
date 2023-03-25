using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    PrefabDatabase prefabDB;
    List<NavMeshSurface> surfaces = new List<NavMeshSurface>();

    [SerializeField]
    int mazeX = 10;

    [SerializeField]
    int mazeY = 10;

    [SerializeField]
    Transform mazeGroup;

    [SerializeField]
    Transform enemyGroup;

    MazeCell[,] mazeCellMap;

    Stack<MazeCell> pathFindingCells = new Stack<MazeCell>();

    //int enemylimits = 10;

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        mazeCellMap = new MazeCell[mazeX, mazeY];

        for (int x = 0; x < mazeX; x++)
        {
            for (int y = 0; y < mazeY; y++)
            {
                MazeCell cell = Instantiate(prefabDB.prefabList[0], mazeGroup).GetComponent<MazeCell>();
                cell.transform.position = new Vector3(cell.mazeSize * x, 0, cell.mazeSize * y);

                mazeCellMap[x, y] = cell;

                cell.Init(x, y);

                if (Random.Range(0, 100) < 3) {
                    GameObject enemy = Instantiate(prefabDB.prefabList[1], cell.gameObject.transform.position, Quaternion.identity, enemyGroup);

                    if (Vector3.Distance(enemy.transform.position, GameObject.FindWithTag("Player").transform.position) < 15) {
                        Destroy(enemy);
                    }
                }

                surfaces.Add(cell.GetComponent<NavMeshSurface>());
            }
        }

        RecursiveBackTracking(mazeCellMap[Random.Range(0, mazeX), Random.Range(0, mazeY)]);

        foreach (NavMeshSurface surface in surfaces)
        {
            surface.BuildNavMesh();
        }
    }

    void RecursiveBackTracking(MazeCell selectedCell)
    {
        selectedCell.isVisited = true;

        List<MazeCell> neighborUnvisitedCells = new List<MazeCell>();

        // Check left
        if (selectedCell.locX - 1 >= 0)
        {
            MazeCell checkingNeighborCell = mazeCellMap[selectedCell.locX - 1, selectedCell.locY];

            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
            }
        }

        // Check right
        if (selectedCell.locX + 1 < mazeX)
        {
            MazeCell checkingNeighborCell = mazeCellMap[selectedCell.locX + 1, selectedCell.locY];

            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
            }
        }

        // Check down
        if (selectedCell.locY - 1 >= 0)
        {
            MazeCell checkingNeighborCell = mazeCellMap[selectedCell.locX, selectedCell.locY - 1];

            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
            }
        }

        // Check up
        if (selectedCell.locY + 1 < mazeY)
        {
            MazeCell checkingNeighborCell = mazeCellMap[selectedCell.locX, selectedCell.locY + 1];

            if (!checkingNeighborCell.isVisited)
            {
                neighborUnvisitedCells.Add(checkingNeighborCell);
            }
        }

        if (neighborUnvisitedCells.Count > 0)
        {
            MazeCell nextSelectedCell = neighborUnvisitedCells[Random.Range(0, neighborUnvisitedCells.Count)];

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

            MazeCell nextSelectedCell = pathFindingCells.Pop();
            RecursiveBackTracking(nextSelectedCell);
        }
        else
        {
            Debug.Log("Generation Done");
        }
    }
}
