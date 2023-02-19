using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLV2 : EnemyBase
{
    protected override void TimerContent()
    {
        nav.SetDestination(target.position);

        hp = Mathf.Min(hp + Time.deltaTime, hpTotal);
    }

    protected override void Damaged(float damage)
    {
        hp = Mathf.Max(0, hp - damage * 2); // So hp will never drop below 0

        if (hp == 0)
        {
            Death();
        }
    }
}
