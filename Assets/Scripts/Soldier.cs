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
    /// ������ ������ ���⿡���� ������ ����
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
    /// �������� �޾��� �� ���� �ִϸ��̼� �۵�
    /// </summary>
    /// <param name="_damage"></param>
    public override void GetDamage(float _damage)
    {
        base.GetDamage(_damage);

        anim.SetTrigger("Hit");
    }

    /// <summary>
    /// ������ Player�� üũ���� �� �Ѿ��� �߻��ϴ� �Լ�
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

                Vector3 moveDir = (targetPos - trsMuzzle.position).normalized;//�Ѿ��� ������ muzzle���� Ÿ����ġ�� �߻�
                rigid.velocity = moveDir * bulletSpeed;
                //Vector3.MoveTowards(obj.transform.position, targetPos, bulletSpeed * Time.deltaTime);

                isShoot = true;//�Ѿ� �߻� ������ Ʈ����
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
    /// �Ѿ� �߻� ������ �Լ�
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
