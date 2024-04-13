using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Enemy
{
    private float shootTimer = 2;
    private float shootTime = 2;
    private bool isShoot = false;
    private bool readyShoot = false;
    private bool goShoot = false;
    private float readyShootTime = 0.7f;
    private float readyShootTimer = 0f;
    [SerializeField] Transform trsMuzzle;
    [SerializeField] GameObject enemyBullet;
    [SerializeField] private float bulletSpeed = 5f;
    private bool isDeath = false;

    public override void Awake()
    {
        base.Awake();
        isShoot = true;
    }

    public override void Update()
    {
        base.Update();
        //readyShooting();
        shoot();
        shootCoolTime();

        if (curHp < 1)
        {
            gameObject.layer = LayerMask.NameToLayer("Nodamage");
            anim.SetTrigger("Death");
            isDeath = true;
        }
    }

    /// <summary>
    /// 솔져의 움직임 방향에따라 스케일 조정
    /// </summary>
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

    /// <summary>
    /// 데미지를 받았을 때 솔져 애니메이션 작동
    /// </summary>
    /// <param name="_damage"></param>
    public override void GetDamage(float _damage)
    {
        base.GetDamage(_damage);

        anim.SetTrigger("Hit");
    }

    /// <summary>
    /// 솔져가 Player를 체크했을 때 총알을 발사하는 함수
    /// </summary>
    private void shoot()
    {
        if (isDeath == true) return;

        if (isCheckPlayer == true)
        {
            readyShoot = true;
            if (isShoot == false)
            {
                GameObject bullets = GameObject.Find("Bullets");
                Transform trsBullets = bullets.transform;

                anim.SetTrigger("Shoot");
                GameObject obj = Instantiate(enemyBullet, trsMuzzle.position, Quaternion.identity, trsBullets);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();

                GameObject objPlayer = GameObject.Find("Player");

                Vector3 moveDir = (targetPos - trsMuzzle.position).normalized;//총알의 물리를 muzzle에서 타겟위치로 발사
                rigid.velocity = moveDir * bulletSpeed;
                //Vector3.MoveTowards(obj.transform.position, targetPos, bulletSpeed * Time.deltaTime);

                isShoot = true;//총알 발사 딜레이 트리거
            }
            
        }
    }

    private void readyShooting()
    {
        if (readyShoot == true)
        {
            readyShootTime -= Time.deltaTime;
            if (readyShootTime < 0)
            {
                readyShootTime = readyShootTimer;
                goShoot = true;
            }
        }
    }

    /// <summary>
    /// 총알 발사 딜레이 함수
    /// </summary>
    private void shootCoolTime()
    {
        //if (goShoot == false) return;

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
