using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spr;
    TrailRenderer trail;
    Camera mainCam;

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float dashPower = 5.0f;
    private Vector3 moveDir;
    private float dashTime = 0.0f;
    private float dashLimitTime = 0.5f;
    private float dashCoolTime = 3.0f;
    private bool isDashCool = false;
    [SerializeField] private Transform trsLeftHand;
    [SerializeField] private Transform trsRightHand;
    [SerializeField] private GameObject trsGun;
    private SpriteRenderer sprGun;
    private Transform trsMuzzle;
    [SerializeField] GameObject objBullet;
    private float bulletSpeed = 50.0f;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
    }

    void Start()
    {
        mainCam = Camera.main;
        sprGun = trsGun.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        checkDashTime();

        move();
        dash();
        dashCoolTimer();
        checkMousePoint();
        checkShoot();
    }

    /// <summary>
    /// 대쉬를 했을 때, move의 물리를 적용하지 않기 위해 만든 타이머
    /// </summary>
    private void checkDashTime()
    {
        if (dashTime > 0.0f)
        {
            dashTime -= Time.deltaTime;
            if (dashTime < 0.0f)
            {
                dashTime = 0.0f;
            }
        }
    }

    /// <summary>
    /// 플레이어가 움직이는 함수
    /// </summary>
    private void move()
    {
        if (dashTime > 0.0f) return;//플레이어가 대쉬 했을 때 리턴

        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.y = Input.GetAxisRaw("Vertical") * moveSpeed;
        rigid.velocity = moveDir;
        anim.SetFloat("Horizontal", moveDir.x);
        anim.SetFloat("Vertical", moveDir.y);

        if (moveDir.y > 0)
        {
            sprGun.sortingOrder = 1;
        }
        else
        {
            sprGun.sortingOrder = 3;
        }
    }

    /// <summary>
    /// 스페이스를 누르면 대쉬하는 함수
    /// </summary>
    private void dash()
    {
        if (isDashCool == true) return;//대쉬가 쿨이면 대쉬가 작동하지 않음

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dashTime = dashLimitTime;//대쉬를 했을 때 velocitiy가 다른 함수에 적용되지 않게 하기 위함

            gameObject.layer = LayerMask.NameToLayer("Nodamage");
            spr.color = new Color(1, 1, 1, 0.4f);
            if (moveDir.x > 0)
            {
                rigid.AddForce(new Vector2(2, 0) * dashPower, ForceMode2D.Impulse);
            }
            else if (moveDir.x < 0)
            {
                rigid.AddForce(new Vector2(-2, 0) * dashPower, ForceMode2D.Impulse);
            }

            if (moveDir.y > 0)
            {
                rigid.AddForce(new Vector2(0, 2) * dashPower, ForceMode2D.Impulse);
            }
            else if (moveDir.y < 0)
            {
                rigid.AddForce(new Vector2(0, -2) * dashPower, ForceMode2D.Impulse);
            }

            isDashCool = true;//대쉬 쿨타임 활성화
            Invoke("returnSituation", 0.8f);
        }
    }

    /// <summary>
    /// 대쉬 후 무적상태 해제함수
    /// </summary>
    private void returnSituation()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        spr.color = new Color(1, 1, 1, 1);
    }

    /// <summary>
    /// 대쉬를 했을 때 대쉬의 쿨 타임을 관리하는 함수
    /// </summary>
    private void dashCoolTimer()
    {
        if (isDashCool == true && dashCoolTime != 0f)
        {
            dashCoolTime -= Time.deltaTime;
            if (dashCoolTime < 0f)
            {
                dashCoolTime = 3.0f;
                isDashCool = false;
            }
        }
    }

    /// <summary>
    /// 마우스의 위치를 파악해 총의 방향을 결졍하는 함수
    /// </summary>
    private void checkMousePoint()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mousePos);
        float mouseX = mouseWorldPos.x;
        Vector3 handScale = trsLeftHand.transform.localScale;
        Vector3 distanceMouseToPlayer = mouseWorldPos - transform.position;
        Vector3 direction = Vector3.right;
        if (distanceMouseToPlayer.x < 0)//왼쪽
        {
            handScale.x = -1;
            direction = Vector3.left;
        }
        else if (distanceMouseToPlayer.x > 0)//오른쪽
        {
            handScale.x = 1;
            direction = Vector3.right;
        }
        trsLeftHand.transform.localScale = handScale;

        float angle = Quaternion.FromToRotation(direction, distanceMouseToPlayer).eulerAngles.z;
        trsLeftHand.localEulerAngles = new Vector3(trsLeftHand.localEulerAngles.x, -trsLeftHand.localEulerAngles.y, angle);

        //Debug.Log($"{mouseX}");
    }

    private void checkShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 shootDir = (mousePos - trsMuzzle.position).normalized;

            shoot(shootDir);
        }
    }

    private void shoot(Vector3 _shootDir)
    {
        GameObject bullet = Instantiate(objBullet, trsMuzzle.position, Quaternion.identity);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = _shootDir * bulletSpeed;
    }
}
