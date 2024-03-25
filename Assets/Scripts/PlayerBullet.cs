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
            scEnemy.GetDamage(damage);
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