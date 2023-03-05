using UnityEngine;
using System.Collections;

public class ArrowAnimation : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    float height;

    Vector3 pos;

    private void Start()
    {
        pos = transform.position;
    }
    void Update()
    {
        float newX = Mathf.Sin(Time.time * speed) * height + pos.x;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}