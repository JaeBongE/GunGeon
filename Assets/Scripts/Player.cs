using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spr;
    TrailRenderer trail;

    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float dashPower = 5.0f;
    private Vector3 moveDir;
    private float dashTime = 0.0f;
    private float dashLimitTime = 0.5f;
    private float dashCoolTime = 3.0f;
    private bool isDashCool = false;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
    }

    void Start()
    {

    }

    void Update()
    {
        checkDashTime();
        
        move();
        dash();
        dashCoolTimer();
    }

    /// <summary>
    /// �뽬�� ���� ��, move�� ������ �������� �ʱ� ���� ���� Ÿ�̸�
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
    /// �÷��̾ �����̴� �Լ�
    /// </summary>
    private void move()
    {
        if (dashTime > 0.0f) return;//�÷��̾ �뽬 ���� �� ����

        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.y = Input.GetAxisRaw("Vertical") * moveSpeed;
        rigid.velocity = moveDir;
        anim.SetFloat("Horizontal", moveDir.x);
        anim.SetFloat("Vertical", moveDir.y);
    }

    private void dash()
    {
        if (isDashCool == true) return;//�뽬�� ���̸� �뽬�� �۵����� ����

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dashTime = dashLimitTime;//�뽬�� ���� �� velocitiy�� �ٸ� �Լ��� ������� �ʰ� �ϱ� ����

            gameObject.layer = LayerMask.NameToLayer("Nodamage");
            spr.color = new Color(1, 1, 1, 0.4f);
            trail.enabled = true;
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

            isDashCool = true;//�뽬 ��Ÿ�� Ȱ��ȭ
            Invoke("returnSituation", 0.8f);
        }
    }


    private void returnSituation()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        spr.color = new Color(1, 1, 1, 1);
        trail.enabled = false;
        trail.Clear();
    }

    /// <summary>
    /// �뽬�� ���� �� �뽬�� �� Ÿ���� �����ϴ� �Լ�
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
}
