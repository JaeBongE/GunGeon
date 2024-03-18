using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    protected Camera mainCam;
    Player player;

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
    }

    private void Update()
    {
        getGun();
        checkGunUi();

        gunDelay();
        reLoad();
    }

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
            }
        }
    }


    public void CreateBullet()
    {
        if (isReload == true) return;

        if (isShoot == true) return;

        GameObject obj = Instantiate(objBullet, trsMuzzle.position, Quaternion.identity);
        Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 shootDir = (mousePos - trsMuzzle.position).normalized;

        rigid.AddForce(shootDir * bulletSpeed, ForceMode2D.Impulse);
        //rigid.velocity = shootDir * bulletSpeed;

        curBullet--;
        isShoot = true;

        if (curBullet <= 0f)
        {
            isReload = true;
        }

    }

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

    private void reLoad()
    {
        if (curBullet == maxBullet) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            isReload = true;
        }
        
        if (isReload == true)
        {
            reLoadUi.SetActive(true);
            ReloadUi scReload = reLoadUi.GetComponentInChildren<ReloadUi>();
            reloadTimer += Time.deltaTime;
            if (reloadTimer > reloadMaxTimer)
            {
                reloadTimer = 0;
                curBullet = maxBullet;
                isReload = false;
                reLoadUi.SetActive(false);
            }
            
            scReload.setReload(reloadTimer, reloadMaxTimer);
        }
    }
}
