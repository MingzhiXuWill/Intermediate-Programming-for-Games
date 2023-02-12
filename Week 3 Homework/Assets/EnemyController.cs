using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace CharacterSpace
{
    public class EnemyController : MonoBehaviour
    {
        public Transform Player;

        private NavMeshAgent nav;

        float moveTimer;
        public float moveTimerTotal = 3;

        public float minDistance = 2;
        float distance;

        [SerializeField]
        Animator anim;

        void Start()
        {
            nav = GetComponent<NavMeshAgent>();

            moveTimer = moveTimerTotal;
        }

        void Update()
        {
            distance = Vector3.Distance(Player.position, transform.position);


            anim.SetFloat("Velocity", nav.velocity.magnitude);

            if (distance < minDistance)
            {
                nav.SetDestination(Player.position);
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

}
