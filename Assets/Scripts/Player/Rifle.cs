using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{


    public override void Awake()
    {
        base.Awake();

    }

    public override void Start()
    {
        base.Start();
        player.ChangeGun(false);
        if (PlayerPrefs.GetFloat("maxBullet") == 30f)
        {
            curBullet = PlayerPrefs.GetFloat("curBullet");
            maxBullet = PlayerPrefs.GetFloat("maxBullet");
            //gameManager.SetGunInfor(gameObject, false);
        }
        else
        {
            maxBullet = 30f;
            curBullet = maxBullet;
            gameManager.SetBulletInfor(curBullet, maxBullet);
            //gameManager.SetGunInfor(gameObject, false);
        }
    }

    public override void Update()
    {
        base.Update();
    }

    public override void CreateBullet()
    {
        base.CreateBullet();
        if (isReload == true) return;//������ �߻�x

        if (isShoot == true) return;//�߻� ������

        GameObject bullets = GameObject.Find("Bullets");
        Transform trsBullets = bullets.transform;

        //�Ѿ��� ������ ���� �߻� ����
        GameObject obj = Instantiate(objBullet, trsMuzzle.position, Quaternion.identity, trsBullets);
        Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();

        //muzzle��ġ���� ���콺�� ��ġ�� �Ѿ��� �߻�
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 shootDir = (mousePos - trsMuzzle.position).normalized;

        //rigid.AddForce(shootDir * bulletSpeed, ForceMode2D.Impulse);
        rigid.velocity = shootDir * bulletSpeed;//�Ѿ��� ������ �̿��� ���ϴ� �������� �߻�

        //�Ѿ��� �پ���, �߻� �����̸� �ش�
        curBullet--;
        isShoot = true;


        gameManager.SetBulletInfor(curBullet, maxBullet);//���ӸŴ����� �Ѿ� ���� ����

        //�Ѿ��� 0�� �Ǹ� ������
        if (curBullet <= 0f)
        {
            isReload = true;//������ Ʈ����
        }
    }

    //public void ChangeGun()
    //{
    //    GameObject player = GameObject.Find("Player");
    //    Player scPlayer = player.GetComponent<Player>();
    //    scPlayer.GetGun(Player.typeGun.Rifle);
    //    gameManager.SetBulletInfor(curBullet, maxBullet);
    //    gameManager.SetGunInfor(gameObject, true);
    //    Destroy(gameObject);
    //}
}
