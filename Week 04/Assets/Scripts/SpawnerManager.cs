using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager instance;

    public Bullet prefab_Bullet;
    public EnemyBase[] prefab_Enemies;

    List<Bullet> bulletList = new List<Bullet>();
    List<EnemyBase> enemyList = new List<EnemyBase>();

    [SerializeField]
    Transform bulletGroup;
    [SerializeField]
    Transform enemyGroup;
    [SerializeField]
    Transform[] spawnPoints;

    struct SpawnRate
    {
        public float spawnRate_EnemyLV1;
        public float spawnRate_EnemyLV2;
        public float spawnRate_EnemyLV3;

        public SpawnRate(int currentLevel)
        {
            if (currentLevel == 0)
            {
                spawnRate_EnemyLV1 = 0.0002f;
                spawnRate_EnemyLV2 = 0.0001f;
                spawnRate_EnemyLV3 = 0;
            }
            else if (currentLevel == 1)
            {
                spawnRate_EnemyLV1 = 0.0003f;
                spawnRate_EnemyLV2 = 0.0002f;
                spawnRate_EnemyLV3 = 0.0001f;
            }
            else
            {
                spawnRate_EnemyLV1 = 0;
                spawnRate_EnemyLV2 = 0;
                spawnRate_EnemyLV3 = 0;
            }
        }
    }

    SpawnRate spawnRate = new SpawnRate(1);

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        if (Random.value < spawnRate.spawnRate_EnemyLV1)
        {
            SpawnEnemies(0, spawnPoint.position, spawnPoint.rotation);
        }
        else if (Random.value < spawnRate.spawnRate_EnemyLV2)
        {
            SpawnEnemies(1, spawnPoint.position, spawnPoint.rotation);
        }
        else if(Random.value < spawnRate.spawnRate_EnemyLV3)
        {
            SpawnEnemies(2, spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void SpawnBullets(Vector3 pos, Quaternion rot)
    {
        Bullet bullet = Instantiate(prefab_Bullet, bulletGroup);

        bullet.transform.position = pos;
        bullet.transform.rotation = rot;

        bulletList.Add(bullet);
    }

    public void RemoveBullet(Bullet bullet)
    {
        bulletList.Remove(bullet);
    }

    public void SpawnEnemies(int id, Vector3 pos, Quaternion rot)
    {
        EnemyBase enemy = Instantiate(prefab_Enemies[id], enemyGroup);
        enemy.transform.position = pos;
        enemy.transform.rotation = rot;

        enemyList.Add(enemy);
    }

    public void RemoveEnemy(EnemyBase enemy)
    {
        if (enemy as EnemyLV1)
        {
            spawnRate.spawnRate_EnemyLV1 += 0.0001f;
        }
        else if (enemy as EnemyLV2)
        {
            spawnRate.spawnRate_EnemyLV2 += 0.0001f;
        }
        else if (enemy as EnemyLV3)
        {
            spawnRate.spawnRate_EnemyLV3 += 0.0001f;
        }
        enemyList.Remove(enemy);
    }
}
