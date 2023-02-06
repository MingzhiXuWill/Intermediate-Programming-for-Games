using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Setting")]

    [SerializeField]
    private GameObject prefabCoin;
    [SerializeField]
    private float initialCoinNumber;
    [SerializeField]
    private float spawnRange;
    [SerializeField]
    private float timerTotal;
    [SerializeField]
    LayerMask layerMask_floor;
    [SerializeField]
    PlayerController player;
    [SerializeField]
    TextMeshProUGUI ScoreUI;

    float timer = 0;

    [HideInInspector]
    public int score = 11;

    void Start()
    {
        for (int i = 0; i < initialCoinNumber; i ++) {
            SpawnCoin();
        }
    }

    void Update()
    {
        if (timer > timerTotal)
        {
            timer = 0;
            SpawnCoin();
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            PointAndClick();
        }
        ScoreUI.text = "Score: " + score;
    }

    /// <summary>
    /// Generate a coin prefab
    /// </summary>
    void SpawnCoin() {
        GameObject newCoin = Instantiate(prefabCoin);

        newCoin.transform.position = new Vector3(Random.Range(-spawnRange, spawnRange), 6, Random.Range(-spawnRange, spawnRange));
    }

    void PointAndClick() {
        Vector2 touchUpPos = Input.mousePosition;

        // Convert 2D to 3D
        Ray currentRay = Camera.main.ScreenPointToRay(touchUpPos);

        RaycastHit hit;
        if (Physics.Raycast(currentRay, out hit, 3000, layerMask_floor))
        {
            Debug.Log("hit " + hit.point);
            player.MoveTo(hit.point);
        }
    }
}
