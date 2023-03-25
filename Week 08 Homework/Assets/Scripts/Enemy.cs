using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    Transform target;
    NavMeshAgent nav;
    Transform lastSeen;
    float chasingTimer = 0;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, target.position) < 1)
        {
            SceneManager.LoadScene("MazeEscapeScene");
        }

        RaycastHit hit;

        if (Physics.Linecast(transform.position, target.position, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                chasingTimer = 3;
                lastSeen = target.transform;

                nav.SetDestination(lastSeen.position);
            }
            else
            {
                if (chasingTimer > 0)
                {
                    chasingTimer -= Time.deltaTime;
                    nav.SetDestination(lastSeen.position);
                }
                else {
                    nav.SetDestination(transform.position);
                }
            }
        }
    }
}
