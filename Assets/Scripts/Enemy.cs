using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHp;
    [SerializeField] protected float curHp;
    Animator anim;
    private void Awake()
    {
        curHp = maxHp;
        anim = GetComponent<Animator>();
    }

    public virtual void GetDamage(float _damage)
    {
        anim.SetTrigger("Hit");
        curHp -= _damage;
    }

    private void Update()
    {
        if (curHp < 1)
        {
            gameObject.layer = LayerMask.NameToLayer("Nodamage");
            anim.SetTrigger("Death");
            Destroy(gameObject, 1f);
        }
    }
}
