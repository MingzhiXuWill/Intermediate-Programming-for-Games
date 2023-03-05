using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Money
    public int defaultPlayerMoney;
    int playerMoney;
    public int PlayerMoney
    {
        get
        {
            return playerMoney;
        }
        set
        {
            if (value < 0)
            {
                playerMoney = 0;
            }
            else
            {
                playerMoney = value;
            }
        }
    }

    // Enemies
    public List<EnemyBase> enemyList;
    [SerializeField]
    List<EnemyBase> enemySpawnList;

    // Health
    public int defaultBaseHealth;
    int baseHealth;
    public int BaseHealth
    {
        get
        {
            return baseHealth;
        }
        set
        {
            if (value < 0)
            {
                baseHealth = 0;
            }
            else
            {
                baseHealth = value;
            }               
        }
    }

    // Level
    public int defaultStartLevel;
    int currentLevel;

    // Score
    int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            if (value < 0)
            {
                score = 0;
            }
            else
            {
                score = value;
            }
        }
    }

    // Text
    [SerializeField]
    TextMeshProUGUI levelText;
    [SerializeField]
    TextMeshProUGUI healthText;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI moneyText;

    // Spawner
    [SerializeField]
    Transform Spawner;

    // Singleton
    public static GameManager instance;

    // BA
    [HideInInspector]
    public BuildArea currentBuildArea;

    // Spawning
    bool isSpawning = false;
    float spawnTimer = 0;
    float spawnTimeTotal = 2.5f;
    float remainingEnemies = 0;

    [SerializeField]
    GameObject startButton;
    [SerializeField]
    GameObject gameoverPanel;

    [SerializeField]
    AudioClip startLevelSound;

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

    void Start()
    {
        currentLevel = defaultStartLevel;
        baseHealth = defaultBaseHealth;
        playerMoney = defaultPlayerMoney;
        score = 0;
    }

    void Update()
    {
        TextUpdate();
        Spawning();
    }

    void Spawning()
    {
        if (isSpawning)
        {
            if (spawnTimer > spawnTimeTotal)
            {
                SpawnEnemy();
                spawnTimer = 0;
                remainingEnemies--;

                if (remainingEnemies <= 0)
                {
                    currentLevel++;
                    isSpawning = false;
                    startButton.SetActive(true);
                }
            }
            else
            {
                spawnTimer += Time.deltaTime;
            }
        }
    }

    void SpawnEnemy()
    {
        SpawnEnemy(enemySpawnList[Random.Range(0, 4)].gameObject);
    }

    void TextUpdate()
    {
        levelText.text = currentLevel.ToString();
        healthText.text = "Base Health  " + BaseHealth;
        scoreText.text = "Score  " + score;
        moneyText.text = playerMoney.ToString();
    }

    public void TakeBaseDamage(int damage) {
        BaseHealth -= damage;
        CheckGameOver();
    }

    void CheckGameOver()
    {
        if (BaseHealth == 0)
        {
            foreach (EnemyBase enemy in enemyList)
            {
                //enemyList.Remove(enemy);
                Destroy(enemy);
            }

            gameoverPanel.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void SpawnEnemy(GameObject enemy)
    {
        EnemyBase thisEnemy = Instantiate(enemy, Spawner.position, Quaternion.identity).GetComponent<EnemyBase>();

        thisEnemy.ReceiveUpgrade(currentLevel - 1);

        enemyList.Add(thisEnemy);
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
    }

    public bool CostMoney(int amount)
    {
        if (amount <= playerMoney)
        {
            playerMoney -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ButtonClick(int buttonNumber)
    {
        if (buttonNumber == 0)
        {
            if (currentBuildArea.currentTowerBase.upgradeTower[0] != null)
            {
                currentBuildArea.BuildTower(currentBuildArea.currentTowerBase.upgradeTower[0]);
            }

        }
        else if (buttonNumber == 1)
        {
            if (currentBuildArea.currentTowerBase.upgradeTower[1] != null)
            {
                currentBuildArea.BuildTower(currentBuildArea.currentTowerBase.upgradeTower[1]);
            }
        }
        else if (buttonNumber == 2)
        {
            currentBuildArea.SellTower();
        }
    }

    public void StartLevel()
    {
        remainingEnemies = 10 + currentLevel * 5;
        isSpawning = true;
        spawnTimeTotal = Mathf.Clamp(2.5f - currentLevel * 0.3f, 1f, 2.5f);

        SoundManager.instance.PlaySound(startLevelSound);

        startButton.SetActive(false);
    }
}
