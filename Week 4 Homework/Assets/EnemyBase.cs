using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyBase : MonoBehaviour
{
    protected GameObject Player;

    private NavMeshAgent nav;

    public int health = 20;

    float moveTimer;
    public float moveTimerTotal = 3;

    public float minDistance = 2;
    float distance;

    [SerializeField]
    Animator anim;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);

            TakeDamage();
        }
    }

    protected virtual void TakeDamage()
    {
        health -= 1;
        CheckDeath();
    }

    protected virtual void GivePoints()
    {
        Player.GetComponent<PlayerController>().score += 1;
    }


    protected virtual void CheckDeath()
    {
        if (health <= 0) {
            GivePoints();
            Destroy(gameObject);
        }
    }

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        Player = GameObject.FindGameObjectWithTag("Player");

        moveTimer = moveTimerTotal;
    }

    void Update()
    {
        distance = Vector3.Distance(Player.transform.position, transform.position);


        anim.SetFloat("Velocity", nav.velocity.magnitude);

        if (distance < minDistance)
        {
            nav.SetDestination(Player.transform.position);
        }
        else if (moveTimer > moveTimerTotal)
        {
            moveTimer = 0;

            nav.speed = 3;
            nav.SetDestination(transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
        }
        else
        {
            moveTimer += Time.deltaTime;
        }

        if (nav.velocity.magnitude < 0.1f)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("Velocity", nav.velocity.magnitude);
        }
    }
}


