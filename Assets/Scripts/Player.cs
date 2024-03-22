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

    [Header("스탯")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float dashPower = 5.0f;
    private Vector3 moveDir;
    [SerializeField] private float maxHp = 3;
    [SerializeField] private float curHp = 0;


    [Header("기타")]
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

        if (sprGun == null) return;
        else
        {
            if (moveDir.y > 0)//포폴 문서에 적기
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
    /// 스페이스를 누르면 대쉬하는 함수
    /// </summary>
    private void dash()
    {
        if (isDashCool == true) return;//대쉬가 쿨이면 대쉬가 작동하지 않음

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dashTime = dashLimitTime;//대쉬를 했을 때 velocitiy가 다른 함수에 적용되지 않게 하기 위함

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
        hitBox.layer = LayerMask.NameToLayer("PlayerHitBox");
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
    private void checkMousePoint()//포폴 문서에 적기
    {
        if (trsGun == null) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCam.transform.position.z;
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mousePos);
        Vector3 distanceMouseToPlayer = mouseWorldPos - transform.position;
        Vector3 direction = Vector3.right;
        Vector3 gunScale = trsGun.localScale;

        float angle = 0f;
        if (distanceMouseToPlayer.x < 0)//마우스 위치 왼쪽 / 총은 오른손
        {
            anim.SetBool("LookFront", false);
            sprGun.sortingOrder = 3;

            //손의 각도
            angle = Quaternion.FromToRotation(-direction, distanceMouseToPlayer).eulerAngles.z;


            //총
            trsGun.SetParent(trsRightHand);
            trsGun.localPosition = Vector3.zero;
            gunScale.x = -1;
            trsGun.localScale = gunScale;

            if (angle > 270f && angle < 320f)
            {
                anim.SetBool("LookFront", true);
                sprGun.sortingOrder = 1;//포폴 문서에 적기
            }

            anim.SetBool("AimRight", false);
            anim.SetBool("AimLeft", true);

        }
        else if (distanceMouseToPlayer.x > 0)//마우스 위치 오른쪽 / 총은 왼손
        {
            anim.SetBool("LookFront", false);
            sprGun.sortingOrder = 3;

            //손의 각도
            angle = Quaternion.FromToRotation(direction, distanceMouseToPlayer).eulerAngles.z;

            //총
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
    /// 총을 쏘는 함수, 총이 없다면 return
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
    /// 총을 획득하는 함수 *미완
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
    /// 적에게 닿으면 hp가 1씩 달고 0이 되면 죽는 함수
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
