using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    SpriteRenderer spr;

    public override void Awake()
    {
        base.Awake();

        spr = GetComponent<SpriteRenderer>();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void move()
    {
        Vector3 Pos = gameObject.transform.position;
        Vector3 scale = gameObject.transform.localScale;
        base.move();

        if (Pos.x > targetPos.x)
        {
            scale.x = 1f;
        }
        else
        {
            scale.x = -1f;
        }
        gameObject.transform.localScale = scale;
        checkPlayer();
    }

    private void checkPlayer()
    {
        if (isCheckPlayer == true)
        {
            anim.SetTrigger("Move");
        }
    }
}
