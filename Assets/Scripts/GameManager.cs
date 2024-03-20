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
    }

    void Update()
    {
        setGunUi();
        setGunBulletUi();
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
}
