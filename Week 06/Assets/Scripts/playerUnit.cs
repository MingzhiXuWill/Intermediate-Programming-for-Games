using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class playerUnit : MonoBehaviour, IMover
{
    Rigidbody rb;

    float speed = 1000;

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        var pos = Position;
        pos.y = 0.3f;
        Position = pos;

        var direction = -transform.localPosition;
        var newVel = direction * speed * Time.deltaTime;
        newVel.y = 0;
        if (newVel.sqrMagnitude > 1)
        {
            newVel = newVel.normalized * 1;
        }
        newVel.y = rb.velocity.y;

        rb.velocity = newVel;
    }

    public void Remove()
    {

    }
}
