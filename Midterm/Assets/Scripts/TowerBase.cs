using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    Transform projectileTransform;

    [SerializeField]
    Projectile myProjectile;

    EnemyBase targetEnemy;

    [SerializeField]
    Sprite projectileSprite;

    public string towerName;    

    public float fireRate;
    private float fireTimer;
    public int damage;
    public float projectileSpeed;
    public int cost;
    public float range;

    public bool canFire;
    public bool canFrostTrap;

    public GameObject[] upgradeTower = new GameObject[2];

    [SerializeField]
    List<AudioClip> fireSoundList;
    [SerializeField]
    public AudioClip buildSound;

    void Start()
    {
        projectileTransform = transform.Find("ProjectileTransform");
    }

    void Update()
    {
        NormalFire();

        FrostTrap();
    }

    void NormalFire()
    {
        if (canFire)
        {
            if (targetEnemy != null)
            {
                if (transform.position.x > targetEnemy.transform.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }

            if (fireTimer > fireRate)
            {
                targetEnemy = GetClosestEnemy();

                if (targetEnemy != null)
                {
                    fireTimer = 0;
                    FireProjectile();

                    SoundManager.instance.PlaySound(fireSoundList[Random.Range(1, (fireSoundList.Count - 1))]); 
                }
            }
            else
            {
                fireTimer += Time.deltaTime;
            }
        }
    }

    EnemyBase GetClosestEnemy() 
    {
        EnemyBase closest = null;
        foreach (EnemyBase enemy in GameManager.instance.enemyList)
        {
            if (WithinRange(enemy))
            {
                if (closest == null)
                {
                    closest = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.GetPosition()) < Vector3.Distance(transform.position, closest.GetPosition()))
                    {
                        closest = enemy;
                    }
                }
            }
        }

        return closest;
    }

    bool WithinRange(EnemyBase target) 
    {
        return (Vector3.Distance(transform.position, target.GetPosition()) <= range);
    }

    void FireProjectile()
    {
        Projectile projectile = Instantiate(myProjectile, projectileTransform.position, Quaternion.identity);
        projectile.SetUp(targetEnemy, projectileSpeed, damage, projectileSprite);
    }

    public void FrostTrap()
    {
        bool enemyWithinRange = false;

        if (canFrostTrap)
        {
            foreach (EnemyBase enemy in GameManager.instance.enemyList)
            {
                if (WithinRange(enemy))
                {
                    enemy.isFrosted = true;
                    enemyWithinRange = true;
                }
            }
        }

        if (enemyWithinRange)
        {
            if (fireTimer > fireRate)
            {
                fireTimer = 0;
                SoundManager.instance.PlaySound(fireSoundList[Random.Range(1, (fireSoundList.Count - 1))]);
            }
            else
            {
                fireTimer += Time.deltaTime;
            }
        }
    }
}
