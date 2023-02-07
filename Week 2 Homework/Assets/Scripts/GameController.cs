using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    [SerializeField]
    GameObject foodPrefab;
    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    float xBoundary;
    [SerializeField]
    float yBoundary;

    [SerializeField]
    float enemySpawnTime;
    [SerializeField]
    int enemySpawnMax;

    [HideInInspector]
    public float enemySpawnTimer = 0;
    [HideInInspector]
    public int enemySpawnCount = 0;

    [SerializeField]
    float foodSpawnTime;
    [SerializeField]
    int foodSpawnMax;

    [HideInInspector]
    public float foodSpawnTimer = 0;
    [HideInInspector]
    public int foodSpawnCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (enemySpawnCount < enemySpawnMax) {
            if (enemySpawnTimer < enemySpawnTime)
            {
                enemySpawnTimer += Time.deltaTime;
            }
            else
            {
                enemySpawnTimer = 0;
                enemySpawnCount--;

                bool spawned = false;
                while (!spawned) {
                    Vector3 randomPos = new Vector3(Random.Range(-xBoundary, xBoundary), Random.Range(-yBoundary, yBoundary), 0);

                    if (Vector3.Distance(playerController.gameObject.transform.position, randomPos) > 10) 
                    {
                        Instantiate(enemyPrefab, randomPos, Quaternion.identity);

                        spawned = true;
                    }
                }
            }
        }

        if (foodSpawnCount < foodSpawnMax)
        {
            if (foodSpawnTimer < foodSpawnTime)
            {
                foodSpawnTimer += Time.deltaTime;
            }
            else
            {
                foodSpawnTimer = 0;
                foodSpawnCount--;

                bool spawned = false;
                while (!spawned)
                {
                    Vector3 randomPos = new Vector3(Random.Range(-xBoundary, xBoundary), Random.Range(-yBoundary, yBoundary), 0);

                    if (Vector3.Distance(playerController.gameObject.transform.position, randomPos) > 10)
                    {
                        Instantiate(foodPrefab, randomPos, Quaternion.identity);

                        spawned = true;
                    }
                }
            }
        }
    }
}
