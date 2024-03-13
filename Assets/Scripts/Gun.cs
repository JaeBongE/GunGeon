using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    protected Camera mainCam;

    [SerializeField] protected GameObject objBullet;
    [SerializeField] protected float bulletSpeed = 20f;
    protected Transform trsMuzzle;

    protected virtual void Awake()
    {
        trsMuzzle = gameObject.transform.GetChild(0);
    }

    protected virtual void Start()
    {
        mainCam = Camera.main;
    }

    public void CreateBullet()
    {
        GameObject obj = Instantiate(objBullet, trsMuzzle.position, Quaternion.identity);
        Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 shootDir = (mousePos - trsMuzzle.position).normalized;

        rigid.AddForce(shootDir * bulletSpeed, ForceMode2D.Impulse);
        //rigid.velocity = shootDir * bulletSpeed;
    }
}
