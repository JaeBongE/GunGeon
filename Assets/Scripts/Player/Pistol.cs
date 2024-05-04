using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        getGun();
    }

    public override void CreateBullet()
    {
        base.CreateBullet();
        if (isReload == true) return;//장전시 발사x

        if (isShoot == true) return;//발사 딜레이

        GameObject bullets = GameObject.Find("Bullets");
        Transform trsBullets = bullets.transform;

        //총알의 물리를 통해 발사 구현
        GameObject obj = Instantiate(objBullet, trsMuzzle.position, Quaternion.identity, trsBullets);
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
        

        gameManager.SetBulletInfor(curBullet, maxBullet);//게임매니저로 총알 정보 전달

        //총알이 0이 되면 재장전
        if (curBullet <= 0f)
        {
            isReload = true;//재장전 트리거
        }
    }

    private void getGun()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D playerColl = Physics2D.OverlapBox(coll.bounds.center, coll.bounds.size, 0, LayerMask.GetMask("Player"));
            if (playerColl != null)
            {
                //player.GetGun(gameObject);
                Player scPlayer = playerColl.GetComponent<Player>();
                scPlayer.GetGun(Player.typeGun.Pistol);
                eIcon.SetActive(false);
                gameManager.SetBulletInfor(curBullet, maxBullet);
                gameManager.SetGunInfor(gameObject, true);
            }

            Destroy(gameObject);
        }
    }
}
