using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cosplayer : MonoBehaviour
{
    NavMeshAgent nav;

    float moveTimer = 0;
    float moveTimerTotal = 3;

    float hungryValue = 0;
    float HungryValue
    {
        get { return hungryValue; }
        set { hungryValue = Mathf.Max(0, hungryValue); }
    }

    float hungryValueTotal = 1000;

    [SerializeField]
    Animator anim;

    Transform restaurant;

    bool isDying;

    void Start()
    {
        hungryValue = hungryValueTotal * 0.6f;
        nav = GetComponent<NavMeshAgent>();

        restaurant = GameObject.Find("Food_MaidCoffee").transform;
    }

    void Update()
    {
        if (!isDying)
        {
            if (HungryValue > 0)
            {
                HungryValue -= 100f * Time.deltaTime;
            }

            if (moveTimer > moveTimerTotal)
            {
                moveTimer = 0;

                if (hungryValue < hungryValueTotal / 2)
                {
                    nav.SetDestination(restaurant.position);
                }
                else
                {
                    nav.SetDestination(transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
                    nav.speed = 3;
                }

                anim.SetBool("isWalking", true);
                anim.SetFloat("Velocity", nav.velocity.magnitude);
            }
            else
            {
                moveTimer += Time.deltaTime;
            }
            Debug.Log(nav.velocity.magnitude);


            if (hungryValue == 0)
            {
                isDying = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Booth_Food") && HungryValue < hungryValueTotal / 2)
        {
            //Eat food at the booth
            anim.SetTrigger("Buy");
            //ResetMoveTimer();
            nav.isStopped = true;

            //Recover health
            HungryValue = hungryValueTotal;
        }
    }
}
