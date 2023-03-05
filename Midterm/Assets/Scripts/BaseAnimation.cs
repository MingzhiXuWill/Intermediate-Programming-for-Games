using UnityEngine;
using System.Collections;

public class BaseAnimation : MonoBehaviour
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
        float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}