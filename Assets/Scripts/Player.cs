using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player Instance;
    GameManager gameManager;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spr;
    Camera mainCam;

    [Header("����")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float dashPower = 5.0f;
    private Vector3 moveDir;
    [SerializeField] private float maxHp = 3;
    [SerializeField] private float curHp = 0;


    [Header("��Ÿ")]
    [SerializeField] GameObject hitBox;
    [SerializeField] private Transform trsLeftHand;
    [SerializeField] private Transform trsRightHand;
    private float dashTime = 0.0f;
    private float dashLimitTime = 0.5f;
    private float dashCoolTime = 3.0f;
    private bool isDashCool = false;
    private GameObject objGun;
    private Transform trsGun;
    private SpriteRenderer sprGun;
    [SerializeField] GameObject reLoadUi;
    [SerializeField] GameObject pisTol;
    private bool isPistol = false;
    [SerializeField] Transform trsBack;
    public GameObject GetReloadUi()
    {
        return reLoadUi;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        reLoadUi.SetActive(false);
        curHp = maxHp;
        //trsGun = gameObject.transform.Find("Pistol");
    }

    void Start()
    {
        mainCam = Camera.main;
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        checkDashTime();

        move();
        dash();
        dashCoolTimer();
        checkMousePoint();
        //checkShoot();
        shoot();


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

        if (sprGun == null) return;
        else
        {
            if (moveDir.y > 0)//���� ������ ����
            {
                sprGun.sortingOrder = 1;
            }
            else
            {
                sprGun.sortingOrder = 3;
            }
        }

    }

    /// <summary>
    /// �����̽��� ������ �뽬�ϴ� �Լ�
    /// </summary>
    private void dash()
    {
        if (isDashCool == true) return;//�뽬�� ���̸� �뽬�� �۵����� ����

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dashTime = dashLimitTime;//�뽬�� ���� �� velocitiy�� �ٸ� �Լ��� ������� �ʰ� �ϱ� ����

            gameObject.layer = LayerMask.NameToLayer("Nodamage");
            hitBox.layer = LayerMask.NameToLayer("Nodamage");
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

            isDashCool = true;//�뽬 ��Ÿ�� Ȱ��ȭ
            Invoke("returnSituation", 0.8f);
        }
    }

    /// <summary>
    /// �뽬 �� �������� �����Լ�
    /// </summary>
    private void returnSituation()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        hitBox.layer = LayerMask.NameToLayer("PlayerHitBox");
        spr.color = new Color(1, 1, 1, 1);
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

    /// <summary>
    /// ���콺�� ��ġ�� �ľ��� ���� ������ �����ϴ� �Լ�
    /// </summary>
    private void checkMousePoint()//���� ������ ����
    {
        if (trsGun == null) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCam.transform.position.z;
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mousePos);
        Vector3 distanceMouseToPlayer = mouseWorldPos - transform.position;
        Vector3 direction = Vector3.right;
        Vector3 gunScale = trsGun.localScale;

        float angle = 0f;
        if (distanceMouseToPlayer.x < 0)//���콺 ��ġ ���� / ���� ������
        {
            anim.SetBool("LookFront", false);
            sprGun.sortingOrder = 3;

            //���� ����
            angle = Quaternion.FromToRotation(-direction, distanceMouseToPlayer).eulerAngles.z;


            //��
            trsGun.SetParent(trsRightHand);
            trsGun.localPosition = Vector3.zero;
            gunScale.x = -1;
            trsGun.localScale = gunScale;

            if (angle > 270f && angle < 320f)
            {
                anim.SetBool("LookFront", true);
                sprGun.sortingOrder = 1;//���� ������ ����
            }

            anim.SetBool("AimRight", false);
            anim.SetBool("AimLeft", true);

        }
        else if (distanceMouseToPlayer.x > 0)//���콺 ��ġ ������ / ���� �޼�
        {
            anim.SetBool("LookFront", false);
            sprGun.sortingOrder = 3;

            //���� ����
            angle = Quaternion.FromToRotation(direction, distanceMouseToPlayer).eulerAngles.z;

            //��
            trsGun.SetParent(trsLeftHand);
            trsGun.localPosition = Vector3.zero;
            gunScale.x = 1;
            trsGun.localScale = gunScale;

            if (angle > 40f && angle < 90f)
            {
                anim.SetBool("LookFront", true);
                sprGun.sortingOrder = 1;
            }

            anim.SetBool("AimRight", true);
            anim.SetBool("AimLeft", false);
        }

        if (distanceMouseToPlayer.y > 0)
        {
            if (trsGun == null) return;

            anim.SetBool("AimFront", true);
            anim.SetBool("AimBehind", false);
        }
        else if (distanceMouseToPlayer.y < 0)
        {
            if (trsGun == null) return;

            anim.SetBool("AimFront", false);
            anim.SetBool("AimBehind", true);
        }

        trsRightHand.localEulerAngles = new Vector3(trsRightHand.localEulerAngles.x, trsRightHand.localEulerAngles.y, angle);
        trsLeftHand.localEulerAngles = new Vector3(trsLeftHand.localEulerAngles.x, trsLeftHand.localEulerAngles.y, angle);

    }

    /// <summary>
    /// ���� ��� �Լ�, ���� ���ٸ� return
    /// </summary>
    private void shoot()
    {
        if (trsGun == null) return;

        if (Input.GetMouseButton(0))
        {
            Gun gun = trsGun.GetComponent<Gun>();
            gun.CreateBullet();
        }
    }


    /// <summary>
    /// ���� ȹ���ϴ� �Լ� *�̿�
    /// </summary>
    /// <param name="_gun"></param>
    public void GetGun(GameObject _gun)
    {
        if (_gun.name == "Pistol")
        {
            _gun.transform.SetParent(trsLeftHand);
            _gun.transform.localPosition = Vector3.zero;

            trsGun = _gun.transform;
            sprGun = trsGun.GetComponent<SpriteRenderer>();

            isPistol = true;
        }

        if (_gun.name != "Pistol" && isPistol == true)
        {
            trsRightHand.localEulerAngles = Vector3.zero;
            trsLeftHand.localEulerAngles = Vector3.zero;
            pisTol.transform.SetParent(trsBack);
            pisTol.SetActive(false);

            _gun.transform.SetParent(trsLeftHand);
            _gun.transform.localPosition = Vector3.zero;

            trsGun = _gun.transform;
            sprGun = trsGun.GetComponent<SpriteRenderer>();
        }

        objGun = _gun;

        gameManager.setGunInfor(objGun, true);
    }

    /// <summary>
    /// ������ ������ hp�� 1�� �ް� 0�� �Ǹ� �״� �Լ�
    /// </summary>
    public void GetDamage()
    {
        if (curHp > 0)
        {
            curHp--;

            gameManager.getPlayerHp(maxHp, curHp);
            if (curHp < 1)
            {
                anim.SetTrigger("isDeath");
            }
        }
    }

}
