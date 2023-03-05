using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "EnemyScriptableObject")]
public class EnemyScriptableObject : ScriptableObject
{
    public int maxHealth;
    public float speed;
    public int damage;
    public int points;
    public string animatorLocation;
}
