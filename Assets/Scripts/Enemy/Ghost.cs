using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    [SerializeField] Transform trsMuzzle;
    [SerializeField] GameObject enemyBullet;
    [SerializeField] private float bulletSpeed = 5f;
    private bool isShoot = false;

    //private void OnDestroy()
    //{
    //    shoot();
    //}

    public override void Awake()
    {
        base.Awake();
    }

    //public override void Update()
    //{
    //    base.Update();

    //    if(curHp == 0)
    //    {
    //        anim.SetTrigger("Death");
    //        //shoot();
    //    }
    //}

    public override void move()
    {
        Vector3 Pos = gameObject.transform.position;
        Vector3 scale = gameObject.transform.localScale;
        base.move();
        if (isCheckPlayer == false)
        {
            if (Pos.x > AstarRandomPos.x)
            {
                scale.x = 1f;
            }
            else
            {
                scale.x = -1f;
            }
            gameObject.transform.localScale = scale;
        }
        else
        {
            if (Pos.x > targetPos.x)
            {
                scale.x = 1f;
            }
            else
            {
                scale.x = -1f;
            }
            gameObject.transform.localScale = scale;
        }
        
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

    //public override void GetDamage(float _damage)
    //{
    //    base.GetDamage(_damage);

    //    hitBox.layer = LayerMask.NameToLayer("Nodamage");
    //    Invoke("returnColor", 1f);
    //    //base.GetDamage(_damage);

    //    //hitBox.layer = LayerMask.NameToLayer("Nodamage");
    //    //anim.SetTrigger("Hit");
    //    //spr.color = Color.red;
    //    //Invoke("returnColor", 1f);
    //}

    //private void returnColor()
    //{
    //    spr.color = Color.white;
    //    hitBox.layer = LayerMask.NameToLayer("EnemyHitBox");
    //}

    private void shoot()
    {
        if (isShoot == false)
        {
            auido.PlayOneShot(auido.clip);

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

            isShoot = true;
        }
        
    }

    public override void death()
    {
        base.death();
        shoot();
    }
}
