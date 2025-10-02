using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage = 2.0f;
    EnemyData EnemyData;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if(enemy != null )
            {
                enemy.TakeDamage(Damage);
            }

            Destroy(gameObject);
        }
    }
}
