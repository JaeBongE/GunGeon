using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    protected Camera mainCam;
    Player player;
    GameManager gameManager;

    protected Collider2D coll;
    protected Rigidbody2D rigid;
    [SerializeField] protected GameObject objBullet;
    [SerializeField] protected float bulletSpeed = 20f;
    protected Transform trsMuzzle;
    [SerializeField] protected float maxBullet = 5f;
    [SerializeField] protected float curBullet = 0f;
    [SerializeField] protected float maxTimer = 2f;
    [SerializeField] protected float timer = 0f;
    protected bool isShoot = false;
    [SerializeField] protected float reloadMaxTimer = 3f;
    [SerializeField] protected float reloadTimer = 0f;
    protected bool isReload = false;
    GameObject reLoadUi;
    [SerializeField] GameObject eIcon;

    public enum GunType
    {
        Pistol,
        Rafle,
    }

    public GunType type;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.Player.ToString())
        {
            eIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.Player.ToString())
        {
            eIcon.SetActive(false);
        }
    }

    protected virtual void Awake()
    {
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        trsMuzzle = gameObject.transform.GetChild(0);
        curBullet = maxBullet;
        //eIcon.SetActive(false);
    }

    protected virtual void Start()
    {
        mainCam = Camera.main;
        player = Player.Instance;
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        getGun();
        
        checkGunUi();

        gunDelay();
        reLoad();
    }

    /// <summary>
    /// 총의 재장전 이미지 가져오기
    /// </summary>
    private void checkGunUi()
    {
        if (reLoadUi == null)
        {
            //reLoadUi = GameObject.Find("ReloadUi");//actvie False 오브젝트는 찾을수가 없음
            //Transform trsPlayer = Transform.FindAnyObjectByType<Player>(FindObjectsInactive.Include).transform;
            //reLoadUi = trsPlayer.Find("ReloadUi").gameObject;
            reLoadUi = player.GetReloadUi();
        }
    }

    /// <summary>
    /// 플레이어가 e를 눌렀을 때 총이 장비되고 총의 정보를 등록한다
    /// </summary>
    private void getGun()
    {
        //if (type == GunType.Pistol) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D playerColl = Physics2D.OverlapBox(coll.bounds.center, coll.bounds.size, 0, LayerMask.GetMask("Player"));
            if (playerColl != null)
            {
                player.GetGun(gameObject);
                eIcon.SetActive(false);
                gameManager.setBulletInfor(curBullet, maxBullet);
            }
        }
    }

    /// <summary>
    /// 총알을 발사하는 함수
    /// </summary>
    public void CreateBullet()
    {
        if (isReload == true) return;//장전시 발사x

        if (isShoot == true) return;//발사 딜레이

        //총알의 물리를 통해 발사 구현
        GameObject obj = Instantiate(objBullet, trsMuzzle.position, Quaternion.identity);
        Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();

        //muzzle위치에서 마우스의 위치로 총알을 발사
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 shootDir = (mousePos - trsMuzzle.position).normalized;

        //rigid.AddForce(shootDir * bulletSpeed, ForceMode2D.Impulse);
        rigid.velocity = shootDir * bulletSpeed;//총알의 물리를 이용해 원하는 방향으로 발사

        //총알이 줄어들고, 발사 딜레이를 준다
        curBullet--;
        isShoot = true;

        gameManager.setBulletInfor(curBullet, maxBullet);//게임매니저로 총알 정보 전달

        //총알이 0이 되면 재장전
        if (curBullet <= 0f)
        {
            isReload = true;//재장전 트리거
        }

    }

    /// <summary>
    /// 총알의 발사 딜레이
    /// </summary>
    private void gunDelay()
    {
        if (isShoot == true)
        {
            timer += Time.deltaTime;
            if (timer > maxTimer)
            {
                timer = 0;
                isShoot = false;
            }
        }
    }

    /// <summary>
    /// 총알을 다 쓰거나 r을 눌렀을 때 재장전
    /// </summary>
    private void reLoad()
    {
        if (curBullet == maxBullet) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            isReload = true;//재장전 트리거
        }
        
        //재장전시 UI, 총알 갯수 표현
        if (isReload == true)
        {
            reLoadUi.SetActive(true);
            ReloadUi scReload = reLoadUi.GetComponentInChildren<ReloadUi>();
            reloadTimer += Time.deltaTime;
            if (reloadTimer > reloadMaxTimer)
            {
                reloadTimer = 0;
                curBullet = maxBullet;
                gameManager.setBulletInfor(curBullet, maxBullet);
                isReload = false;
                reLoadUi.SetActive(false);
            }
            
            //재장전 이미지 표시
            scReload.setReload(reloadTimer, reloadMaxTimer);
        }
    }
}
