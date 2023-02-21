using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLV3 : EnemyBase
{
    private bool secondChance = true;

    protected override void Start()
    {
        base.Start();
    }

    protected override void TimerContent()
    {
        nav.SetDestination(target.position);

        hp = Mathf.Min(hp + Time.deltaTime, hpTotal);
    }

    public override void Damaged(float damage)
    {
        hp = Mathf.Max(0, hp - damage * 2); // So hp will never drop below 0

        if (hp == 0)
        {
            Death();
        }
    }
    protected override void Death()
    {
        if (secondChance)
        {
            secondChance = false;
            hp = hpTotal;
            nav.speed *= 2;
        }
        else {
            SpawnerManager.instance.RemoveEnemy(this);
            Destroy(gameObject);
        }
    }
}
