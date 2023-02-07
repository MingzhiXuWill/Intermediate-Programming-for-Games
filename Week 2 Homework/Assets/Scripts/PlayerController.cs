using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameController gameController;

    [SerializeField]
    TextMeshProUGUI textScore;

    [SerializeField]
    Color minColor;
    [SerializeField]
    Color maxColor;

    [SerializeField]
    public float speed = 0;

    [SerializeField]
    float xBoundary;
    [SerializeField]
    float yBoundary;

    [SerializeField]
    float minSize;
    [SerializeField]
    float maxSize;
    [SerializeField]
    int minPoint = 0;
    [SerializeField]
    int maxPoint;

    [SerializeField]
    int pointPerFood;
    [SerializeField]
    int minPointKill;

    private int currentPoint = 0;
    private float currentSize;

    private Vector2 mousePosition;

    void Start()
    {
        UpdateSize();
    }

    void Update()
    {
        textScore.text = "Score " + currentPoint;

        UpdateSize();

        GetComponent<Renderer>().material.color = Color.Lerp(minColor, maxColor, (float)currentPoint / maxPoint);

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = Vector2.MoveTowards(transform.position, mousePosition, speed * Time.deltaTime);

        if (transform.position.x >= xBoundary)
        {
            transform.position -= new Vector3(transform.position.x - xBoundary, 0, 0);
        }
        else if (transform.position.x <= -xBoundary)
        {
            transform.position -= new Vector3(transform.position.x + xBoundary, 0, 0);
        }
        if (transform.position.y >= yBoundary)
        {
            transform.position -= new Vector3(0, transform.position.y - yBoundary, 0);
        }
        else if (transform.position.y <= -yBoundary)
        {
            transform.position -= new Vector3(0, transform.position.y + yBoundary, 0);
        }
    }

    void UpdateSize()
    {
        currentSize = Mathf.Lerp(minSize, maxSize, (float)currentPoint / (float)maxPoint);

        Debug.Log((float)currentPoint / (float)maxPoint);

        transform.localScale = new Vector3(currentSize, currentSize, 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            currentPoint += pointPerFood;

            gameController.foodSpawnCount--;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            float point = other.gameObject.GetComponent<EnemyController>().CurrentPoint;

            if (Mathf.Min(maxPoint, currentPoint) > point)
            {
                if (Mathf.Sqrt(point) < minPointKill)
                {
                    currentPoint += minPointKill;
                }
                else
                {
                    currentPoint += (int)Mathf.Sqrt(point);
                }
                gameController.enemySpawnCount--;
                Destroy(other.gameObject);
            }
            else
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
}
