using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.EnemyHitBox.ToString())
        {
            Enemy scEnemy = collision.gameObject.GetComponentInParent<Enemy>();
            if (scEnemy == null)
            {
                
            }
            scEnemy.GetDamage(damage);
            Destroy(gameObject);
        }
        
        if (collision.gameObject.tag == GameTag.Wall.ToString())
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        Destroy(gameObject, 3f);
    }

    
}
