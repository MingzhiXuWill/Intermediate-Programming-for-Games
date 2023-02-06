using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
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
    int minPoint;
    [SerializeField]
    int maxPoint;

    private int currentPoint;
    private float currentSize;

    private Vector3 direction;

    void Start()
    {
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

        currentPoint = Random.Range(minPoint, maxPoint);

        currentSize = Mathf.Lerp(minSize, maxSize, (float)currentPoint / maxPoint);

        transform.localScale = new Vector3(currentSize, currentSize, 1);
    }

    void Update()
    {
        transform.position = transform.position + direction * speed * Time.deltaTime;

        GetComponent<Renderer>().material.color = Color.Lerp(minColor, maxColor, (float)currentPoint / maxPoint);

        if (transform.position.x > xBoundary)
        {
            direction.x = -Mathf.Abs(direction.x);
        }
        else if (transform.position.x < -xBoundary)
        {
            direction.x = Mathf.Abs(direction.x);
        }
        if (transform.position.y > yBoundary)
        {
            direction.y = -Mathf.Abs(direction.y);
        }
        else if (transform.position.y < -yBoundary)
        {
            direction.y = Mathf.Abs(direction.y);
        }
    }
}
