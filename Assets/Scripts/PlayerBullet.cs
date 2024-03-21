using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.EnemyHitBox.ToString())
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == GameTag.Wall.ToString())
        {
            Destroy(gameObject);
        }


    }

    private void Awake()
    {
        Destroy(gameObject, 3f);
    }

    
}
