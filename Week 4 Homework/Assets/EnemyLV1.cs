using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLV1 : EnemyBase
{
    protected override void TakeDamage()
    {
        health -= 2;
        CheckDeath();
    }

    protected override void GivePoints()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().score += 3;
    }
}
