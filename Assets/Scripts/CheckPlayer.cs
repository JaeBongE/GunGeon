using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    Enemy enemy;

    private void Start()
    {
        enemy = Enemy.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.PlayerHitBox.ToString())
        {
            enemy.CheckPlayer();
        }
    }
}
