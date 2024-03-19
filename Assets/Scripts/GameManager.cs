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

    public void setGunInfor(GameObject _gun)
    {
        SpriteRenderer spr = _gun.GetComponent<SpriteRenderer>();
        //gunUi
    }
}
