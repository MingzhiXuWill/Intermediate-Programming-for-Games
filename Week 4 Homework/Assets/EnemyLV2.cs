using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLV2 : EnemyBase
{
    protected override void TakeDamage()
    {
        if (Random.value < 0.5f)
        {
            health -= 2;
        }
        CheckDeath();
    }

    protected override void GivePoints()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().score += 5;
    }
}
