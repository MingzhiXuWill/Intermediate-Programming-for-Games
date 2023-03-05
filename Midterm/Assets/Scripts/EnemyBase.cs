using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected GameObject target;

    private NavMeshAgent nav;

    private Animator animator;

    public bool isFrosted;

    string animatorLocation;

    int health;
    int maxHealth;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            if (value < 0)
            {
                health = 0;
            }
            else
            {
                health = value;
            }
        }
    }
    float speed;
    int damage;
    int points;

    [SerializeField]
    EnemyScriptableObject myEnemySO;

    [SerializeField]
    Transform healthBar;
    [SerializeField]
    Transform displayGroup;

    [SerializeField]
    Color32 normalColor;
    [SerializeField]
    Color32 frostedColor;

    SpriteRenderer spriteRenderer;

    // Sound
    [SerializeField]
    AudioClip HurtSound;
    [SerializeField]
    AudioClip KilledSound;


    void Start()
    {
        maxHealth = myEnemySO.maxHealth;
        speed = myEnemySO.speed;
        damage = myEnemySO.damage;
        points = myEnemySO.points;
        animatorLocation = myEnemySO.animatorLocation;

        health = maxHealth;

        target = GameObject.FindGameObjectWithTag("playerBase");
        animator = transform.Find("Sprite").GetComponent<Animator>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();

        animator.runtimeAnimatorController = Resources.Load(animatorLocation) as RuntimeAnimatorController;

        nav = GetComponent<NavMeshAgent>();
        nav.speed = speed;
        nav.SetDestination(target.transform.position);
    }

    public void ReceiveUpgrade(int modifier)
    {
        maxHealth = Mathf.Clamp(myEnemySO.maxHealth + modifier * 4, myEnemySO.maxHealth, (int)(myEnemySO.maxHealth * 2.5f));

        health = maxHealth;

        speed = Mathf.Clamp(myEnemySO.speed + modifier * 0.05f, myEnemySO.speed, myEnemySO.speed * 1.5f);
    }

    void Update()
    {
        HealthBarUpdate();

        if (isFrosted)
        {
            nav.speed = speed / 2;
            spriteRenderer.color = frostedColor;
            isFrosted = false;
        }
        else
        {
            spriteRenderer.color = normalColor;
            nav.speed = speed;
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 1f)
        {
            GameManager.instance.TakeBaseDamage(damage);
            Death();
        }
    }

    void HealthBarUpdate()
    {
        displayGroup.localRotation = Quaternion.Euler(25, 0, 0);
        healthBar.transform.localScale = new Vector3((float)health / (float)maxHealth, 1, 1);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        SoundManager.instance.PlaySound(HurtSound);

        CheckDeath();
    }

    void GivePoints()
    {
        GameManager.instance.Score += points;
        GameManager.instance.PlayerMoney += points;
    }

    void CheckDeath()
    {
        if (health <= 0)
        {
            GivePoints();
            Death();
        }
    }

    void Death()
    {
        SoundManager.instance.PlaySound(KilledSound);
        GameManager.instance.enemyList.Remove(this);
        Destroy(gameObject);
    }

    public Vector3 GetPosition()
    {
        return (gameObject.transform.position);
    }
}



