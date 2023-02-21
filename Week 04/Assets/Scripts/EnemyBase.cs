using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected Transform target;
    protected float hp = 0;

    [SerializeField]
    protected float hpTotal = 100;

    private float timer = 0;
    [SerializeField]
    private float timerTotal = 0;

    protected NavMeshAgent nav;

    protected virtual void Start()
    {
        hp = hpTotal;
        target = GameManager.instance.player;
        nav = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        TimerTool();
    }

    public virtual void Damaged(float damage)
    {
        hp = Mathf.Max(0, hp - damage); // So hp will never drop below 0

        if (hp == 0) {
            Death();
        }
    }

    protected virtual void Death()
    {
        SpawnerManager.instance.RemoveEnemy(this);
        Destroy(gameObject);
    }

    private void TimerTool() {
        if (timer > timerTotal)
        {
            timer = 0;
            TimerContent();
        }
        else {
            timer += Time.deltaTime;
        }
    }

    protected virtual void TimerContent()
    {
        // Acutall content
    }
}
