using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    Player player;

    [Header("��")]
    [SerializeField] Image gunUi;
    [SerializeField] TMP_Text gunBulletUi;
    private float maxBullet;
    private float curBullet;
    private bool isGunImageOn = false;

    [Header("�÷��̾�")]
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

    [Header("��Ÿ")]
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
        enemies = GameObject.FindGameObjectsWithTag("Enemy");//enemy�±��� ���ӿ�����Ʈ�� ã�� �ִ´�
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
    /// ���� ���� �� UI�� ���� ǥ��
    /// </summary>
    private void setGunUi()
    {
        if (isGunImageOn == true)
        {
            gunUi.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// �Ѿ��� ȭ�鿡 ǥ���ϴ� UI
    /// </summary>
    private void setGunBulletUi()
    {
        gunBulletUi.text = ($"{curBullet} / {maxBullet}");
    }

    /// <summary>
    /// PlayerHP�� �޾ƿͼ� UI�� ü���� ǥ��
    /// </summary>
    private void setPlayerHp()//ü���� �ִ�ġ�� �þ�ٸ� �����ʿ�
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
    /// Dash��Ÿ�� UI ����
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
    /// ȭ��� enemy�� �ִ��� Ȯ�� 
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

        if (allClear == true)//enemy�� ���ٸ� ��Ż�� ����
        {
            potal.SetActive(true);
        }
    }

    /// <summary>
    /// �����κ��� �Ѿ��� ������ �޾ƿ´�
    /// </summary>
    /// <param name="_curBullet"> ���� �Ѿ��� ��</param>
    /// <param name="_maxBullet"> �ִ� �Ѿ��� ��</param>
    public void setBulletInfor(float _curBullet, float _maxBullet)
    {
        curBullet = _curBullet;
        maxBullet = _maxBullet;
    }

    /// <summary>
    /// ���� ȹ������ ��, ���� ������ �޾ƿ� UI�� ���� �̹����� ǥ���ϰ� ��������1���� ���డ���� �˸���.
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
    /// Player�� ü�� ������ �޾ƿ´�
    /// </summary>
    /// <param name="_maxHp"> �ִ� ü��</param>
    /// <param name="_curHp"> ���� ü��</param>
    public void getPlayerHp(float _maxHp, float _curHp)
    {
        curHp = _curHp;
        maxHp = _maxHp;
    }

    /// <summary>
    /// Player�� �뽬 ��Ÿ�� ������ �޾ƿ´�
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
    /// Enemy�� ������� �� �ڽ��� �迭���� �����ϰ� Ȯ���Ѵ�
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
