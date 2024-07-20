using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage2Manager : MonoBehaviour
{
    public static Stage2Manager Instance;
    GameManager gameManager;

    GameObject player;
    [SerializeField] Image gunUI;

    [Header("터미널 눌렀을 때")]
    [SerializeField] GameObject wall;
    [SerializeField] private float maxY;
    private bool isTerminal = false;
    [SerializeField] GameObject choiceUI;

    [Header("적이 처치 되었을 때")]
    [SerializeField] GameObject wall2;
    [SerializeField] private float maxX;
    [SerializeField] GameObject checkEnemy;
    [SerializeField] GameObject checkEnemy1;
    private bool isDeathEnemy = false;

    [SerializeField] GameObject objRifle;


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

        if (PlayerPrefs.HasKey("Rifle"))
        {
            PlayerPrefs.DeleteKey("Rifle");
        }
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        Player scPlayer = player.GetComponent<Player>();
        //if (PlayerPrefs.GetString("Gun") == "Pistol")
        //{
        //    scPlayer.GetGun(Player.typeGun.Pistol);
        //}
        //else if (PlayerPrefs.GetString("Gun") == "Rifle")
        //{
        //    scPlayer.GetGun(Player.typeGun.Rifle);
        //}

        scPlayer.GetGun(Player.typeGun.Pistol);


        GameObject objGun = GameObject.Find("LeftHand").transform.GetChild(0).gameObject;
        SpriteRenderer sprGun = objGun.GetComponent<SpriteRenderer>();
        gunUI.sprite = sprGun.sprite;
        gunUI.gameObject.SetActive(true);

        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        moveWall();

        checkDeathEnemy();
    }

    private void moveWall()
    {
        if (isTerminal == true)
        {
            float wallY = wall.transform.position.y;

            if (wallY > maxY)
            {
                wallY = maxY;
                //Destroy(wall);
            }

            //벽이 위로 이동하면서 문이 열린다
            wallY += Time.deltaTime;
            wall.transform.position = new Vector3(wall.transform.position.x, wallY, wall.transform.position.z);
        }
    }

    public void CheckTerminal(bool _terminal)
    {
        isTerminal = _terminal;
    }

    private void checkDeathEnemy()
    {
        if (checkEnemy == null && checkEnemy1 == null)
        {
            isDeathEnemy = true;
        }

        if (isDeathEnemy == true)
        {
            if (wall2 == null) return;

            float wallX = wall2.transform.position.x;

            if (wallX > maxX)
            {
                wallX = maxX;
                //Destroy(wall);
            }

            //벽이 위로 이동하면서 문이 열린다
            wallX += Time.deltaTime;
            wall2.transform.position = new Vector3(wallX, wall2.transform.position.y, wall2.transform.position.z);
        }
    }

    public void ChangeGun()
    {
        player = GameObject.Find("Player");
        Player scPlayer = player.GetComponent<Player>();
        scPlayer.ChangeGun(Player.typeGun.Rifle);
        gameManager.SetGunInfor(objRifle, false);

        PlayerPrefs.SetString("Rifle", "Get");
        //Rifle scRifle = objRifle.GetComponent<Rifle>();
        //scRifle.ChangeGun();
    }

    public void OpenChoiceUI()
    {
        choiceUI.SetActive(true);
    }
}
