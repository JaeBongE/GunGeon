using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Enemy
{

    public override void Update()
    {
        base.Update();

        if (curHp < 1)
        {
            gameObject.layer = LayerMask.NameToLayer("Nodamage");
            anim.SetTrigger("Death");
            Destroy(gameObject, 1f);
        }
    }

    public override void moveRandom()
    {
        if (isMove == true)
        {
            anim.SetTrigger("Move");
        }

        Vector3 Pos = gameObject.transform.position;
        Vector3 scale = gameObject.transform.localScale;
        base.moveRandom();

        if (Pos.x > targetPos.x)
        {
            scale.x = -1f;
        }
        else
        {
            scale.x = 1f;
        }
        gameObject.transform.localScale = scale;

    }

    public override void GetDamage(float _damage)
    {
        base.GetDamage(_damage);

        anim.SetTrigger("Hit");
    }
}
