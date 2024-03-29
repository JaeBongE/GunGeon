using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    Player player;

    [SerializeField] Image gunUi;
    [SerializeField] TMP_Text gunBulletUi;
    private float maxBullet;
    private float curBullet;
    private bool isGunImageOn = false;
    [SerializeField] Image hp3;
    [SerializeField] Image hp2;
    [SerializeField] Image hp1;
    private float maxHp;
    private float curHp;

    GameObject[] enemies;
    [SerializeField] GameObject potal;

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
        gunUi.gameObject.SetActive(false);
    }

    void Start()
    {
        player = Player.Instance;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        potal.SetActive(false);
    }

    void Update()
    {
        setGunUi();
        setGunBulletUi();
        setPlayerHp();
        checkEnemy();
    }


    private void setGunUi()
    {
        if (isGunImageOn == true)
        {
            gunUi.gameObject.SetActive(true);
        }
    }

    private void setGunBulletUi()
    {
        gunBulletUi.text = ($"{curBullet} / {maxBullet}");
    }

    private void setPlayerHp()
    {
        if (curHp < maxHp)
        {
            hp3.gameObject.SetActive(false);
            if (curHp < maxHp - 1)
            {
                hp2.gameObject.SetActive(false);
                if (curHp == 0)
                {
                    hp1.gameObject.SetActive(false);
                }
            }
        }
    }

    private void checkEnemy()
    {
        int count = enemies.Length;
        bool allClear = true;
        for (int iNum = 0; iNum < count; ++iNum)
        {
            if (enemies[iNum] != null)
            {
                allClear = false;
            }
        }
        //allClear

        if (allClear == true)
        {
            potal.SetActive(true);
        }
    }

    public void setBulletInfor(float _curBullet, float _maxBullet)
    {
        curBullet = _curBullet;
        maxBullet = _maxBullet;
    }

    public void setGunInfor(GameObject _gun, bool _isGun)
    {
        SpriteRenderer spr = _gun.GetComponent<SpriteRenderer>();
        gunUi.sprite = spr.sprite;
        if (_isGun == true)
        {
            isGunImageOn = true;
            GameObject objStage1Manager = GameObject.Find("Stage1Manager");
            Stage1Manager scStage1 = objStage1Manager.GetComponent<Stage1Manager>();
            scStage1.isContinue(isGunImageOn);
        }
    }

    public void getPlayerHp(float _maxHp, float _curHp)
    {
        curHp = _curHp;
        maxHp = _maxHp;
    }

    public void CheckoutEnemy(GameObject _obj)
    {
        int count = enemies.Length;
        for (int iNum = 0; iNum < count; ++iNum)
        {
            if (enemies[iNum] == _obj)
            {
                enemies[iNum] = null;
                return;
            }
        }

        checkEnemy();
    }
}
