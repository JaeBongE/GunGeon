using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player Instance;

    GameManager gameManager;
    GameManager gameManagerInstance
    {
        get 
        {
            if (gameManager == null)
            {
                gameManager = GameManager.Instance;
            }
            return gameManager;
        }   
    }

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
    [SerializeField] private bool isNodamage = false;


    [Header("총관련")]
    [SerializeField] GameObject hitBox;
    [SerializeField] private Transform trsLeftHand;
    [SerializeField] private Transform trsRightHand;
    private float dashTime = 0.0f;
    private float dashLimitTime = 0.5f;
    private float dashCoolTime = 3.0f;
    private float dashCoolMaxTime = 3.0f;
    private bool isDashCool = false;
    private GameObject objGun;
    private Transform trsGun;
    private SpriteRenderer sprGun;
    private bool isChange = false;


    [Header("UI")]
    [SerializeField] GameObject reLoadUi;
    private bool isPistol = false;
    [SerializeField] Transform trsBack;

    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();

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
        //trsGun = gameObject.transform.Find("Pistol");

        //if (PlayerPrefs.HasKey("CurHP") == true)
        //{
        //    curHp = PlayerPrefs.GetFloat("CurHP");
        //    GameManager.Instance.SetPlayerHp(maxHp, curHp);
        //}
        //else
        //{
        //    curHp = maxHp;
        //}


        //if (trsLeftHand.GetChild(0).gameObject != null)
        //{
        //    GameObject _gun = trsLeftHand.GetChild(0).gameObject;
        //    if (_gun != null)
        //    {
        //        objGun = _gun;

        //        objGun.transform.SetParent(trsLeftHand);
        //        objGun.transform.localPosition = Vector3.zero;

        //        trsGun = objGun.transform;
        //        sprGun = trsGun.GetComponent<SpriteRenderer>();

        //        isPistol = true;
        //    }
        //}

    }

    private void Start()
    {
        mainCam = Camera.main;
        if (PlayerPrefs.HasKey("CurHP") == true)//스테이지 넘어갈 때 플레이어의 HP를 저장해뒀다가 다음 스테이지에서 불러오기
        {
            curHp = PlayerPrefs.GetFloat("CurHP");
            gameManagerInstance.SetPlayerHp(maxHp, curHp);
        }
        else
        {
            curHp = maxHp;
        }
        gameManagerInstance.SetPlayerHp(maxHp, curHp);
    }

    private void Update()
    {
        checkDashTime();

        move();
        dash();
        dashCoolTimer();
        checkMousePoint();
        //checkShoot();
        shoot();
        checkGun();
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

        //Horizontal, Vertical 입력에 따라 물리를 적용해 움직임
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.y = Input.GetAxisRaw("Vertical") * moveSpeed;
        rigid.velocity = moveDir;

        anim.SetFloat("Horizontal", moveDir.x);
        anim.SetFloat("Vertical", moveDir.y);

        if (sprGun == null) return;
        else // 플레이어가 총을 가지고 있을 때 위아래 움직이는 방향에 따라 총의 sortingOrder값을 변경해 자연스러운 연출
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
            audioSource.PlayOneShot(audioClips[0]);

            dashTime = dashLimitTime;//대쉬를 했을 때 velocitiy가 다른 함수에 적용되지 않게 하기 위함
            dashCoolTime = dashCoolMaxTime;//대쉬 했을 때 쿨타임 돌게 설계해 UI 표현

            gameObject.layer = LayerMask.NameToLayer("Nodamage");//대쉬 했을 때 layer를 바꿔 무적상태 구현
            hitBox.layer = LayerMask.NameToLayer("Nodamage");
            spr.color = new Color(1, 1, 1, 0.4f);

            //캐릭터가 가는 방향에 따라 대쉬 힘의 방향을 결정
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
            Invoke("returnSituation", 0.8f); // 무적 해제
        }
    }

    /// <summary>
    /// 무적상태 해제함수
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
            gameManagerInstance.GetPlayerDash(dashCoolTime, dashCoolMaxTime, true);//게임매니저로 대쉬 쿨 타임 전달해 UI관리

            if (dashCoolTime < 0f)
            {
                dashCoolTime = 0f;
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

            if (angle > 270f && angle < 320f)//총이 위를 향할 때 sortingOrder를 낮춰서 자연스러운 연출 
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

            if (angle > 40f && angle < 90f)//총이 위를 향할 때 sortingOrder를 낮춰서 자연스러운 연출
            {
                anim.SetBool("LookFront", true);
                sprGun.sortingOrder = 1;
            }

            anim.SetBool("AimRight", true);
            anim.SetBool("AimLeft", false);
        }

        if (distanceMouseToPlayer.y > 0)//총의 에임이 위로 향할때
        {
            if (trsGun == null) return;

            anim.SetBool("AimFront", true);
            anim.SetBool("AimBehind", false);
        }
        else if (distanceMouseToPlayer.y < 0)//총의 에임이 아래로 향할때
        {
            if (trsGun == null) return;

            anim.SetBool("AimFront", false);
            anim.SetBool("AimBehind", true);
        }

        //손의 각도를 통해 총이 에임을 따라가게 만듬
        trsRightHand.localEulerAngles = new Vector3(trsRightHand.localEulerAngles.x, trsRightHand.localEulerAngles.y, angle);
        trsLeftHand.localEulerAngles = new Vector3(trsLeftHand.localEulerAngles.x, trsLeftHand.localEulerAngles.y, angle);

    }

    /// <summary>
    /// 총을 쏘는 함수, 총이 없다면 return
    /// </summary>
    private void shoot()
    {
        if (trsGun == null) return;

        if (Input.GetMouseButton(0))//마우스 왼쪽 버튼을 누르면
        {
            Gun gun = trsGun.GetComponent<Gun>();
            gun.CreateBullet();//총알을 발사
        }
    }

    public enum typeGun
    {
        Pistol,
        Rifle,
    }

    /// <summary>
    /// 총을 얻었을 때 or 스테이지를 이동할때 플레이어에게 총을 쥐어주는 함수
    /// </summary>
    /// <param name="_value"></param>
    public void GetGun(typeGun _value)
    {
        Destroy(objGun);

        bool isExist = gameManagerInstance != null;
        if (isExist == false)
        { 
            Debug.LogError("gameManager Empty");
        }

        objGun = Instantiate(gameManagerInstance.GetWeapon(_value));
        objGun.transform.SetParent(trsLeftHand);
        objGun.transform.localPosition = Vector3.zero;
        //objGun.transform.localPosition = trsLeftHand.localEulerAngles;

        trsGun = objGun.transform;
        sprGun = trsGun.GetComponent<SpriteRenderer>();

        isPistol = true;

    }

    private void checkGun()
    {
        if (objGun != null)
        {
            if (objGun.name == "Pistol(Clone)")
            {
                Pistol scPistol = objGun.GetComponent<Pistol>();
                scPistol.HaveGun(true);
            }
        }
    }

    /// <summary>
    /// 총의 변경을 확인해서 총을 쥐어주는 함수
    /// </summary>
    /// <param name="_value"></param>
    public void ChangeGun(typeGun _value)
    {
        Destroy(objGun);

        objGun = Instantiate(gameManagerInstance.GetWeapon(_value));
        objGun.transform.SetParent(trsLeftHand);
        objGun.transform.localPosition = Vector3.zero;
        objGun.transform.localEulerAngles = Vector3.zero;

        trsGun = objGun.transform;
        sprGun = trsGun.GetComponent<SpriteRenderer>();

        //GameObject objManager = GameObject.Find("GameManager");
        //GameManager scManager = objManager.GetComponent<GameManager>();
        //gameManager.SetGunInfor(objGun, false);

        //gameManager.SetGunInfor(objGun, false);

    }

    /// <summary>
    /// 총을 획득하는 함수 *미완
    /// </summary>
    /// <param name="_gun"></param>
    //public void GetGun(GameObject _gun)
    //{
    //    if (objGun != null) return;

    //    Destroy(objGun);

    //    objGun = _gun;
    //    objGun.transform.SetParent(trsLeftHand);
    //    objGun.transform.localPosition = Vector3.zero;

    //    trsGun = objGun.transform;
    //    sprGun = trsGun.GetComponent<SpriteRenderer>();

    //    isPistol = true;



    //    //if (_gun.name == "Pistol")
    //    //{
    //    //    _gun.transform.SetParent(trsLeftHand);
    //    //    _gun.transform.localPosition = Vector3.zero;

    //    //    trsGun = _gun.transform;
    //    //    sprGun = trsGun.GetComponent<SpriteRenderer>();

    //    //    isPistol = true;
    //    //}

    //    //if (_gun.name != "Pistol" && isPistol == true)
    //    //{
    //    //    trsRightHand.localEulerAngles = Vector3.zero;
    //    //    trsLeftHand.localEulerAngles = Vector3.zero;
    //    //    pisTol.transform.SetParent(trsBack);
    //    //    pisTol.SetActive(false);

    //    //    _gun.transform.SetParent(trsLeftHand);
    //    //    _gun.transform.localPosition = Vector3.zero;

    //    //    trsGun = _gun.transform;
    //    //    sprGun = trsGun.GetComponent<SpriteRenderer>();
    //    //}


    //    //gameManager.SetGunInfor(_gun, true);
    //}

    /// <summary>
    /// 적에게 닿으면 hp가 1씩 달고 0이 되면 죽는 함수
    /// </summary>
    public void GetDamage()
    {
        if (isNodamage == true) return;

        if (curHp > 0)
        {
            curHp--;
            //PlayerPrefs.SetFloat("CurHP", curHp);

            if (curHp < 1)//피가 0이 되면 
            {
                audioSource.PlayOneShot(audioClips[2]);
                anim.SetTrigger("isDeath");//죽는다
                gameManagerInstance.PlayerDeath();
            }

            gameObject.layer = LayerMask.NameToLayer("Nodamage");//한대 맞으면 레이어를 바꿔줘서 다단히트로 들어오는 것을 방지 및 빨간색 히트 연출
            hitBox.layer = LayerMask.NameToLayer("Nodamage");
            spr.color = Color.red;

            gameManagerInstance.SetPlayerHp(maxHp, curHp);//게임매니저에 체력 상태를 전달해 UI표시
            //gameManager.SetPlayerHpUI();

            Invoke("returnSituation", 1f);//원래 상태 복귀

        }
    }

    public void CheckNodamage(bool _isNodamage)
    {
        isNodamage = _isNodamage;
    }

    public void Heal()
    {
        audioSource.PlayOneShot(audioClips[1]);

        curHp = maxHp;
        gameManagerInstance.SetPlayerHp(maxHp, curHp);
    }

    public void ChangeGun(bool _isChange)
    {
        isChange = _isChange;
    }
}
