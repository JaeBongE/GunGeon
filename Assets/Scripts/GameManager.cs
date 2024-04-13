using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    Player player;

    [Header("총")]
    [SerializeField] Image gunUi;
    [SerializeField] TMP_Text gunBulletUi;
    private float maxBullet;
    private float curBullet;
    private bool isGunImageOn = false;

    [Header("플레이어")]
    [SerializeField] Image hp3;
    [SerializeField] Image hp2;
    [SerializeField] Image hp1;
    private float maxHp;
    private float curHp;
    [SerializeField] Image dashImage;
    private float dashCoolTime;
    private float dashCoolMaxTime;
    private bool isDash = false;
    [SerializeField] TMP_Text tmpDashCool;

    [Header("기타")]
    [SerializeField] GameObject pauseUI;
    private bool isPauseOpen = false;

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
    }

    void Start()
    {
        player = Player.Instance;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");//enemy태그의 게임오브젝트를 찾아 넣는다
        gunUi.gameObject.SetActive(false);
        potal.SetActive(false);
        dashImage.fillAmount = 0;
        tmpDashCool.text = "";

    }

    void Update()
    {
        setGunUi();
        setGunBulletUi();
        setPlayerHp();
        setPlayerDash();
        checkEnemy();
        showPauseUI();
    }


    /// <summary>
    /// 총이 있을 때 UI에 총을 표시
    /// </summary>
    private void setGunUi()
    {
        if (isGunImageOn == true)
        {
            gunUi.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 총알을 화면에 표시하는 UI
    /// </summary>
    private void setGunBulletUi()
    {
        gunBulletUi.text = ($"{curBullet} / {maxBullet}");
    }

    /// <summary>
    /// PlayerHP를 받아와서 UI에 체력을 표시
    /// </summary>
    private void setPlayerHp()//체력의 최대치가 늘어난다면 수정필요
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

    /// <summary>
    /// Dash쿨타임 UI 관리
    /// </summary>
    private void setPlayerDash()
    {
        if (isDash == true)
        {
            dashImage.fillAmount = dashCoolTime / dashCoolMaxTime;
            tmpDashCool.text = ($"{dashCoolTime.ToString("F1")}");

            if (dashCoolTime < 0f)
            {
                tmpDashCool.text = "";
            }
        }
    }

    /// <summary>
    /// 화면상 enemy가 있는지 확인 
    /// </summary>
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

        if (allClear == true)//enemy가 없다면 포탈을 연다
        {
            potal.SetActive(true);
        }
    }

    /// <summary>
    /// 총으로부터 총알의 정보를 받아온다
    /// </summary>
    /// <param name="_curBullet"> 현재 총알의 수</param>
    /// <param name="_maxBullet"> 최대 총알의 수</param>
    public void setBulletInfor(float _curBullet, float _maxBullet)
    {
        curBullet = _curBullet;
        maxBullet = _maxBullet;
    }

    /// <summary>
    /// 총을 획득했을 때, 총의 정보를 받아와 UI에 총의 이미지를 표시하고 스테이지1에게 진행가능을 알린다.
    /// </summary>
    /// <param name="_gun"></param>
    /// <param name="_isGun"></param>
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

    /// <summary>
    /// Player의 체력 정보를 받아온다
    /// </summary>
    /// <param name="_maxHp"> 최대 체력</param>
    /// <param name="_curHp"> 현재 체력</param>
    public void getPlayerHp(float _maxHp, float _curHp)
    {
        curHp = _curHp;
        maxHp = _maxHp;
    }

    /// <summary>
    /// Player의 대쉬 쿨타임 정보를 받아온다
    /// </summary>
    /// <param name="_dashCoolTime"></param>
    /// <param name="_dashCoolMaxTime"></param>
    /// <param name="_isDash"></param>
    public void getPlayerDash(float _dashCoolTime, float _dashCoolMaxTime, bool _isDash)
    {
        dashCoolTime = _dashCoolTime;
        dashCoolMaxTime = _dashCoolMaxTime;
        isDash = _isDash;
    }

    /// <summary>
    /// Enemy가 사라졌을 때 자신을 배열에서 삭제하고 확인한다
    /// </summary>
    /// <param name="_obj"></param>
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

    private void showPauseUI()
    {
        PauseUI scPause = pauseUI.GetComponent<PauseUI>();
        GameObject objPauseUI = scPause.setPauseUI();

        //if (isPauseOpen == false)
        //{
        //    objPauseUI.SetActive(false);
        //}
        if (isPauseOpen == false && Input.GetKeyDown(KeyCode.Escape))
        {
            objPauseUI.SetActive(true);
            Time.timeScale = 0f;
            isPauseOpen = true;
        }
        else if (isPauseOpen == true && Input.GetKeyDown(KeyCode.Escape))
        {
            objPauseUI.SetActive(false);
            Time.timeScale = 1f;
            isPauseOpen = false;
        }
    }

    public void checkPauseUI(bool _isOpen)
    {
        isPauseOpen = _isOpen;
    }
}
