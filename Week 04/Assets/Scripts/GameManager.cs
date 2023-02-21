using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public Transform player;

    public static GameManager instance;

    [SerializeField]
    public GameObject ui_GameOverPage;

    [HideInInspector]
    public bool isGameOver = false;

    // Touched by the enemy
    public void GameOver()
    {
        isGameOver = true;
        ui_GameOverPage.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }
}
