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
    Image gunUi;
    TMP_Text gunBulletUi;
    private float maxBullet;
    private float curBullet;
    private bool isGunImageOn = false;
    [SerializeField] List<GameObject> fabWeapon;

    [Header("�÷��̾�")]
    Image hp3;
    Image hp2;
    Image hp1;
    private float maxHp;
    [SerializeField] private float curHp;
    private bool isDeath = false;
    Image dashImage;
    private float dashCoolTime;
    private float dashCoolMaxTime;
    private bool isDash = false;
    TMP_Text tmpDashCool;

    [Header("��Ÿ")]
    GameObject pauseUI;
    GameObject gameOverUI;
    GameObject oneMoreUI;
    private bool isPauseOpen = false;

    GameObject[] enemies;
    GameObject portal;

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

        isDeath = false;

        if (PlayerPrefs.HasKey("curBullet") && PlayerPrefs.HasKey("maxBullet"))
        {
            curBullet = PlayerPrefs.GetFloat("curBullet");
            maxBullet = PlayerPrefs.GetFloat("maxBullet");
        }

        setPlayerHpUI();
    }

    void Update()
    {
        setUI();

        //setGunUi();
        setGunBulletUi();
        setPlayerHpUI();

        //setPlayerHp();

        setPlayerDash();

        checkEnemy();

        showPauseUI();
        //playerDeath();
    }

    private void setUI()
    {
        if (gunUi == null)
        {
            GameObject objPlayerUI = GameObject.Find("PlayerUI");
            if (objPlayerUI == null) return;

            PlayerUI scPlayerUI = objPlayerUI.GetComponent<PlayerUI>();

            (Image _gunUi, TMP_Text _gunBulletUi) gunUI = scPlayerUI.GetGunUI();
            gunUi = gunUI._gunUi;
            gunBulletUi = gunUI._gunBulletUi;

            //gunUi.gameObject.SetActive(false);

            (Image _hp3, Image _hp2, Image _hp1) hpUI = scPlayerUI.GetHpUI();
            hp3 = hpUI._hp3;
            hp2 = hpUI._hp2;
            hp1 = hpUI._hp1;

            (Image _dashImage, TMP_Text _tmpDashCool) dashUI = scPlayerUI.GetDashUI();
            dashImage = dashUI._dashImage;
            tmpDashCool = dashUI._tmpDashCool;

            dashImage.fillAmount = 0;
            tmpDashCool.text = "";

            GameObject objPauseUI = GameObject.Find("PauseUI");
            if (objPauseUI == null) return;

            PauseUI scPauseUI = objPauseUI.GetComponent<PauseUI>();
            (GameObject _pauseUI, GameObject _gameoverUI, GameObject _oneMoreUI) pause = scPauseUI.GetPauseUI();
            pauseUI = pause._pauseUI;
            gameOverUI = pause._gameoverUI;
            oneMoreUI = pause._oneMoreUI;

            portal = GameObject.Find("Portal");
            portal.SetActive(false);
        }
    }


    /// <summary>
    /// ���� ���� �� UI�� ���� ǥ��
    /// </summary>
    //private void setGunUi()
    //{
    //    if (isGunImageOn == true)
    //    {
    //        gunUi.gameObject.SetActive(true);
    //    }
    //}

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
    private void setPlayerHpUI()//ü���� �ִ�ġ�� �þ�ٸ� �����ʿ�
    {
        if (gunUi == null) return;

        if (curHp == 3)
        {
            hp1.gameObject.SetActive(true);
            hp2.gameObject.SetActive(true);
            hp3.gameObject.SetActive(true);
        }

        if (curHp == 2)
        {
            hp1.gameObject.SetActive(true);
            hp2.gameObject.SetActive(true);
            hp3.gameObject.SetActive(false);
        }

        if (curHp == 1)
        {
            hp1.gameObject.SetActive(true);
            hp2.gameObject.SetActive(false);
            hp3.gameObject.SetActive(false);
        }

        if (curHp == 0)
        {
            hp1.gameObject.SetActive(false);
            hp2.gameObject.SetActive(false);
            hp3.gameObject.SetActive(false);
            //isDeath = true;
            //playerDeath();
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
            portal.SetActive(true);
        }
    }

    /// <summary>
    /// �����κ��� �Ѿ��� ������ �޾ƿ´�
    /// </summary>
    /// <param name="_curBullet"> ���� �Ѿ��� ��</param>
    /// <param name="_maxBullet"> �ִ� �Ѿ��� ��</param>
    public void SetBulletInfor(float _curBullet, float _maxBullet)
    {
        //PlayerPrefs.SetFloat("curBullet", _curBullet);
        //PlayerPrefs.SetFloat("maxBullet", _maxBullet);

        curBullet = _curBullet;
        maxBullet = _maxBullet;
    }

    /// <summary>
    /// ���� ȹ������ ��, ���� ������ �޾ƿ� UI�� ���� �̹����� ǥ���ϰ� ��������1���� ���డ���� �˸���.
    /// </summary>
    /// <param name="_gun"></param>
    /// <param name="_isGun"></param>
    public void SetGunInfor(GameObject _gun, bool _isGun)
    {
        SpriteRenderer spr = _gun.GetComponent<SpriteRenderer>();
        gunUi.gameObject.SetActive(true);
        gunUi.sprite = spr.sprite;

        if (_isGun == true)
        {
            isGunImageOn = true;

            GameObject objStage1Manager = GameObject.Find("Stage1Manager");
            if (objStage1Manager == null) return;
            Stage1Manager scStage1 = objStage1Manager.GetComponent<Stage1Manager>();
            scStage1.isContinue(isGunImageOn);
        }
    }

    /// <summary>
    /// Player�� ü�� ������ �޾ƿ´�
    /// </summary>
    /// <param name="_maxHp"> �ִ� ü��</param>
    /// <param name="_curHp"> ���� ü��</param>
    public void SetPlayerHp(float _maxHp, float _curHp)
    {
        curHp = _curHp;
        maxHp = _maxHp;
        setPlayerHpUI();
    }

    /// <summary>
    /// Player�� �뽬 ��Ÿ�� ������ �޾ƿ´�
    /// </summary>
    /// <param name="_dashCoolTime"></param>
    /// <param name="_dashCoolMaxTime"></param>
    /// <param name="_isDash"></param>
    public void GetPlayerDash(float _dashCoolTime, float _dashCoolMaxTime, bool _isDash)
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

        //if (isPauseOpen == false)
        //{
        //    objPauseUI.SetActive(false);
        //}
        if (isPauseOpen == false && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
            isPauseOpen = true;
        }
        else if (isPauseOpen == true && Input.GetKeyDown(KeyCode.Escape))
        {
            oneMoreUI.SetActive(false);
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
            isPauseOpen = false;
        }

    }

    public void PlayerDeath()
    {
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
        //if (isDeath == true)
        //{
        //    isDeath = false;
        //}
    }

    public void CheckPauseUI(bool _isOpen)
    {
        isPauseOpen = _isOpen;
    }

    public GameObject GetWeapon(Player.typeGun _value)
    {
        return fabWeapon[(int)_value];
    }

    public void SaveInfor()
    {
        PlayerPrefs.SetFloat("CurHP", curHp);
        PlayerPrefs.SetFloat("curBullet", curBullet);
        PlayerPrefs.SetFloat("maxBullet", maxBullet);
    }
}
