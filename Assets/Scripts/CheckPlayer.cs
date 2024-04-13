using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.PlayerHitBox.ToString())
        {
            Enemy scEnemy = gameObject.GetComponentInParent<Enemy>();
            scEnemy.CheckPlayer();
        }
    }
}
