using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    protected Camera mainCam;
    protected Player player;
    protected GameManager gameManager;

    protected Collider2D coll;
    protected Rigidbody2D rigid;
    [SerializeField] protected GameObject objBullet;
    [SerializeField] protected float bulletSpeed;
    protected Transform trsMuzzle;
    [SerializeField] protected float maxBullet;
    [SerializeField] protected float curBullet = 0f;
    [SerializeField] protected float maxTimer = 2f;
    [SerializeField] protected float timer = 0f;
    protected bool isShoot = false;
    [SerializeField] protected float reloadMaxTimer = 3f;
    [SerializeField] protected float reloadTimer = 0f;
    protected bool isReload = false;
    protected GameObject reLoadUi;
    [SerializeField] protected GameObject eIcon;
    protected bool isChangeGun = false;

    [SerializeField] protected AudioSource audio;
    [SerializeField] protected AudioClip ReloadSound;



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

    public virtual void Awake()
    {
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        trsMuzzle = gameObject.transform.GetChild(0);
        
        if (PlayerPrefs.HasKey("curBullet") && PlayerPrefs.HasKey("maxBullet"))
        {
            curBullet = PlayerPrefs.GetFloat("curBullet");
            maxBullet = PlayerPrefs.GetFloat("maxBullet");
        }
        else
        {
            curBullet = maxBullet;
        }
        //eIcon.SetActive(false);
    }

    public virtual void Start()
    {
        mainCam = Camera.main;
        player = Player.Instance;
        gameManager = GameManager.Instance;
    }

    public virtual void Update()
    {
        //getGun();
        
        checkGunUi();

        gunDelay();
        reLoad();
    }

    /// <summary>
    /// ���� ������ �̹��� ��������
    /// </summary>
    private void checkGunUi()
    {
        if (reLoadUi == null)
        {
            //reLoadUi = GameObject.Find("ReloadUi");//actvie False ������Ʈ�� ã������ ����
            //Transform trsPlayer = Transform.FindAnyObjectByType<Player>(FindObjectsInactive.Include).transform;
            //reLoadUi = trsPlayer.Find("ReloadUi").gameObject;
            reLoadUi = player.GetReloadUi();
        }
    }

    /// <summary>
    /// �÷��̾ e�� ������ �� ���� ���ǰ� ���� ������ ����Ѵ�
    /// </summary>
    //private void getGun()
    //{

    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        Collider2D playerColl = Physics2D.OverlapBox(coll.bounds.center, coll.bounds.size, 0, LayerMask.GetMask("Player"));
    //        if (playerColl != null)
    //        {
    //            //player.GetGun(gameObject);
    //            Player scPlayer = playerColl.GetComponent<Player>();
    //            //scPlayer.GetGun(gameObject);
    //            eIcon.SetActive(false);
    //            gameManager.SetBulletInfor(curBullet, maxBullet); 
    //            gameManager.SetGunInfor(gameObject, true);

    //        }
    //    }
    //}

    /// <summary>
    /// �Ѿ��� �߻��ϴ� �Լ�
    /// </summary>
    public virtual void CreateBullet()
    {
        if (isReload == true) return;//������ �߻�x

        if (isShoot == true) return;//�߻� ������

        //GameObject bullets = GameObject.Find("Bullets");
        //Transform trsBullets = bullets.transform;

        ////�Ѿ��� ������ ���� �߻� ����
        //GameObject obj = Instantiate(objBullet, trsMuzzle.position, Quaternion.identity, trsBullets);
        //Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();

        ////muzzle��ġ���� ���콺�� ��ġ�� �Ѿ��� �߻�
        //Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        //mousePos.z = 0;
        //Vector3 shootDir = (mousePos - trsMuzzle.position).normalized;

        ////rigid.AddForce(shootDir * bulletSpeed, ForceMode2D.Impulse);
        //rigid.velocity = shootDir * bulletSpeed;//�Ѿ��� ������ �̿��� ���ϴ� �������� �߻�

        ////�Ѿ��� �پ���, �߻� �����̸� �ش�
        //curBullet--;
        //isShoot = true;

        //gameManager.setBulletInfor(curBullet, maxBullet);//���ӸŴ����� �Ѿ� ���� ����

        ////�Ѿ��� 0�� �Ǹ� ������
        //if (curBullet <= 0f)
        //{
        //    isReload = true;//������ Ʈ����
        //}

    }

    /// <summary>
    /// �Ѿ��� �߻� ������
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
    /// �Ѿ��� �� ���ų� r�� ������ �� ������
    /// </summary>
    private void reLoad()
    {
        if (curBullet == maxBullet) return;

        

        if (Input.GetKeyDown(KeyCode.R))
        {
            audio.PlayOneShot(ReloadSound);

            isReload = true;//������ Ʈ����
        }
        
        //�������� UI, �Ѿ� ���� ǥ��
        if (isReload == true)
        {
            reLoadUi.SetActive(true);
            ReloadUi scReload = reLoadUi.GetComponentInChildren<ReloadUi>();
            reloadTimer += Time.deltaTime;
            
            //������ �̹��� ǥ��
            scReload.setReload(reloadTimer, reloadMaxTimer);

            if (reloadTimer > reloadMaxTimer)
            {
                reloadTimer = 0;
                scReload.setReload(reloadTimer, reloadMaxTimer);
                curBullet = maxBullet;
                gameManager.SetBulletInfor(curBullet, maxBullet);
                isReload = false;
                reLoadUi.SetActive(false);
            }
        }
    }
}
