using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.PlayerHitBox.ToString())
        {
            Player scPlayer = collision.gameObject.GetComponentInParent<Player>();
            scPlayer.GetDamage();
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
