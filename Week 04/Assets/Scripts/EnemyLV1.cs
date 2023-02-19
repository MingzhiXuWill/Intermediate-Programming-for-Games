using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLV1 : EnemyBase
{

    protected override void TimerContent()
    {
        nav.SetDestination(target.position);
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
