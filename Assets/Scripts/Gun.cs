using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    protected Camera mainCam;

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
    [SerializeField] GameObject reLoadUi;

    protected virtual void Awake()
    {
        trsMuzzle = gameObject.transform.GetChild(0);
        curBullet = maxBullet;
    }

    protected virtual void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        gunDelay();
        reLoad();
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
