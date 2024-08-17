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

    [Header("����")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float dashPower = 5.0f;
    private Vector3 moveDir;
    [SerializeField] private float maxHp = 3;
    [SerializeField] private float curHp = 0;
    [SerializeField] private bool isNodamage = false;


    [Header("�Ѱ���")]
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
        if (PlayerPrefs.HasKey("CurHP") == true)//�������� �Ѿ �� �÷��̾��� HP�� �����ص״ٰ� ���� ������������ �ҷ�����
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

        //Horizontal, Vertical �Է¿� ���� ������ ������ ������
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.y = Input.GetAxisRaw("Vertical") * moveSpeed;
        rigid.velocity = moveDir;

        anim.SetFloat("Horizontal", moveDir.x);
        anim.SetFloat("Vertical", moveDir.y);

        if (sprGun == null) return;
        else // �÷��̾ ���� ������ ���� �� ���Ʒ� �����̴� ���⿡ ���� ���� sortingOrder���� ������ �ڿ������� ����
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
            audioSource.PlayOneShot(audioClips[0]);

            dashTime = dashLimitTime;//�뽬�� ���� �� velocitiy�� �ٸ� �Լ��� ������� �ʰ� �ϱ� ����
            dashCoolTime = dashCoolMaxTime;//�뽬 ���� �� ��Ÿ�� ���� ������ UI ǥ��

            gameObject.layer = LayerMask.NameToLayer("Nodamage");//�뽬 ���� �� layer�� �ٲ� �������� ����
            hitBox.layer = LayerMask.NameToLayer("Nodamage");
            spr.color = new Color(1, 1, 1, 0.4f);

            //ĳ���Ͱ� ���� ���⿡ ���� �뽬 ���� ������ ����
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
            Invoke("returnSituation", 0.8f); // ���� ����
        }
    }

    /// <summary>
    /// �������� �����Լ�
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
            gameManagerInstance.GetPlayerDash(dashCoolTime, dashCoolMaxTime, true);//���ӸŴ����� �뽬 �� Ÿ�� ������ UI����

            if (dashCoolTime < 0f)
            {
                dashCoolTime = 0f;
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

            if (angle > 270f && angle < 320f)//���� ���� ���� �� sortingOrder�� ���缭 �ڿ������� ���� 
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

            if (angle > 40f && angle < 90f)//���� ���� ���� �� sortingOrder�� ���缭 �ڿ������� ����
            {
                anim.SetBool("LookFront", true);
                sprGun.sortingOrder = 1;
            }

            anim.SetBool("AimRight", true);
            anim.SetBool("AimLeft", false);
        }

        if (distanceMouseToPlayer.y > 0)//���� ������ ���� ���Ҷ�
        {
            if (trsGun == null) return;

            anim.SetBool("AimFront", true);
            anim.SetBool("AimBehind", false);
        }
        else if (distanceMouseToPlayer.y < 0)//���� ������ �Ʒ��� ���Ҷ�
        {
            if (trsGun == null) return;

            anim.SetBool("AimFront", false);
            anim.SetBool("AimBehind", true);
        }

        //���� ������ ���� ���� ������ ���󰡰� ����
        trsRightHand.localEulerAngles = new Vector3(trsRightHand.localEulerAngles.x, trsRightHand.localEulerAngles.y, angle);
        trsLeftHand.localEulerAngles = new Vector3(trsLeftHand.localEulerAngles.x, trsLeftHand.localEulerAngles.y, angle);

    }

    /// <summary>
    /// ���� ��� �Լ�, ���� ���ٸ� return
    /// </summary>
    private void shoot()
    {
        if (trsGun == null) return;

        if (Input.GetMouseButton(0))//���콺 ���� ��ư�� ������
        {
            Gun gun = trsGun.GetComponent<Gun>();
            gun.CreateBullet();//�Ѿ��� �߻�
        }
    }

    public enum typeGun
    {
        Pistol,
        Rifle,
    }

    /// <summary>
    /// ���� ����� �� or ���������� �̵��Ҷ� �÷��̾�� ���� ����ִ� �Լ�
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
    /// ���� ������ Ȯ���ؼ� ���� ����ִ� �Լ�
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
    /// ���� ȹ���ϴ� �Լ� *�̿�
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
    /// ������ ������ hp�� 1�� �ް� 0�� �Ǹ� �״� �Լ�
    /// </summary>
    public void GetDamage()
    {
        if (isNodamage == true) return;

        if (curHp > 0)
        {
            curHp--;
            //PlayerPrefs.SetFloat("CurHP", curHp);

            if (curHp < 1)//�ǰ� 0�� �Ǹ� 
            {
                audioSource.PlayOneShot(audioClips[2]);
                anim.SetTrigger("isDeath");//�״´�
                gameManagerInstance.PlayerDeath();
            }

            gameObject.layer = LayerMask.NameToLayer("Nodamage");//�Ѵ� ������ ���̾ �ٲ��༭ �ٴ���Ʈ�� ������ ���� ���� �� ������ ��Ʈ ����
            hitBox.layer = LayerMask.NameToLayer("Nodamage");
            spr.color = Color.red;

            gameManagerInstance.SetPlayerHp(maxHp, curHp);//���ӸŴ����� ü�� ���¸� ������ UIǥ��
            //gameManager.SetPlayerHpUI();

            Invoke("returnSituation", 1f);//���� ���� ����

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
