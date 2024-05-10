using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    SpriteRenderer spr;
    [SerializeField] Transform trsMuzzle;
    [SerializeField] GameObject enemyBullet;
    [SerializeField] private float bulletSpeed = 5f;

    private void OnDestroy()
    {
        shoot();
    }

    public override void Awake()
    {
        base.Awake();

        spr = GetComponent<SpriteRenderer>();
    }

    public override void Update()
    {
        base.Update();

        if (curHp < 1)
        {
            //gameObject.layer = LayerMask.NameToLayer("Nodamage");
            anim.SetTrigger("Death");
        }
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
            moveSpeed = 5f;
        }
    }

    public override void GetDamage(float _damage)
    {
        base.GetDamage(_damage);

        hitBox.layer = LayerMask.NameToLayer("Nodamage");
        anim.SetTrigger("Hit");
        spr.color = Color.red;
        Invoke("returnColor", 0.7f);
    }

    private void returnColor()
    {
        spr.color = Color.white;
        hitBox.layer = LayerMask.NameToLayer("EnemyHitBox");
    }

    private void shoot()
    {
        GameObject bullets = GameObject.Find("Bullets");
        Transform trsBullets = bullets.transform;

        GameObject obj = Instantiate(enemyBullet, trsMuzzle.position, Quaternion.identity, trsBullets);
        GameObject obj1 = Instantiate(enemyBullet, trsMuzzle.position, Quaternion.identity, trsBullets);
        GameObject obj2 = Instantiate(enemyBullet, trsMuzzle.position, Quaternion.identity, trsBullets);
        GameObject obj3 = Instantiate(enemyBullet, trsMuzzle.position, Quaternion.identity, trsBullets);
        
        Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
        Rigidbody2D rigid1 = obj1.GetComponent<Rigidbody2D>();
        Rigidbody2D rigid2 = obj2.GetComponent<Rigidbody2D>();
        Rigidbody2D rigid3 = obj3.GetComponent<Rigidbody2D>();

        rigid.velocity = Vector3.up * bulletSpeed;
        rigid1.velocity = Vector3.down * bulletSpeed;
        rigid2.velocity = Vector3.left * bulletSpeed;
        rigid3.velocity = Vector3.right * bulletSpeed;
    }
}
