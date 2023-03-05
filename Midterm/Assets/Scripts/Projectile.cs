using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    EnemyBase enemy;

    float projectileSpeed;
    int damage;

    bool Setup = false;

    public void SetUp(EnemyBase enemy, float projectileSpeed, int damage, Sprite sprite)
    {
        this.enemy = enemy;
        this.projectileSpeed = projectileSpeed;
        this.damage = damage;

        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;

        spriteRenderer.sprite = sprite;

        Setup = true;
    }

    private void Update()
    {
        if (Setup) {
            if (enemy == null)
            {
                Destroy(gameObject);
            }

            // Move
            if (enemy != null)
            {
                Vector3 targetPosition = enemy.GetPosition() + new Vector3(0, 0.5f, 0);
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                transform.position += moveDir * projectileSpeed * Time.deltaTime;

                // Rotation
                transform.rotation = Quaternion.LookRotation(moveDir);
                transform.eulerAngles = new Vector3(90, transform.eulerAngles.y - 90, transform.eulerAngles.z);

                // Hits
                if (Vector3.Distance(transform.position, targetPosition) < 1f)
                {
                    enemy.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
