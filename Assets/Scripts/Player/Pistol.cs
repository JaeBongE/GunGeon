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
