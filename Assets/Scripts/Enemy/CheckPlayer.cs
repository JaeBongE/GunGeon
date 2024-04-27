using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    Enemy scEnemy;

    private void Awake()
    {
        scEnemy = gameObject.GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.PlayerHitBox.ToString())
        {
            scEnemy.CheckPlayer();
        }
    }
}
