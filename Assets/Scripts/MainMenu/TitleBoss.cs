using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBoss : MonoBehaviour
{
    Animator anim;
    [SerializeField] private float attackTime = 3f;
    private float attackTimer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        attackTimer = attackTime;
    }

    void Update()
    {
        attackAnimTime();
    }

    private void attackAnimTime()
    {
        attackTime -= Time.deltaTime;
        if (attackTime < 0)
        {
            anim.SetTrigger("Attack");
            attackTime = attackTimer;
        }
    }
}
