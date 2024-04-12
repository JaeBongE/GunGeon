using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Enemy
{
    private float shootTimer = 2;
    private float shootTime = 2;
    private bool isShoot = false;
    [SerializeField] Transform trsMuzzle;
    [SerializeField] GameObject enemyBullet;
    [SerializeField] private float bulletSpeed = 5f;

    public override void Update()
    {
        base.Update();
        shoot();
        shootCoolTime();

        if (curHp < 1)
        {
            gameObject.layer = LayerMask.NameToLayer("Nodamage");
            anim.SetTrigger("Death");
        }
    }

    public override void move()
    {
        if (isMove == true)
        {
            anim.SetTrigger("Move");
        }

        Vector3 Pos = gameObject.transform.position;
        Vector3 scale = gameObject.transform.localScale;
        base.move();

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

    private void shoot()
    {
        if (isCheckPlayer == true)
        {
            if (isShoot == false)
            {
                anim.SetTrigger("Shoot");
                GameObject obj = Instantiate(enemyBullet, trsMuzzle.position, Quaternion.identity);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                GameObject objPlayer = GameObject.Find("Player");
                Vector3 moveDir = (targetPos - trsMuzzle.position).normalized;
                rigid.velocity = moveDir * bulletSpeed;
                //Vector3.MoveTowards(obj.transform.position, targetPos, bulletSpeed * Time.deltaTime);
                isShoot = true;
            }
            
        }
    }

    private void shootCoolTime()
    {
        if (isShoot == true)
        {
            shootTime -= Time.deltaTime;
            if (shootTime < 0)
            {
                shootTime = shootTimer;
                isShoot = false;
            }
        }
        
    }
}
