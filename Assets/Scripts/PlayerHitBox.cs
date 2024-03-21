using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.EnemyHitBox.ToString())
        {
            Player player = GetComponentInParent<Player>();
            player.GetDamage();
        }
    }
}
