using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageManager : MonoBehaviour
{
    public static BossStageManager Instance;
    GameManager gameManager;
    
    GameObject player;

    [SerializeField] GameObject objPistol;
    [SerializeField] GameObject objRifle;

    [Header("�͹̳� ������ ��")]
    [SerializeField] GameObject wall;
    [SerializeField] private float maxX;
    private bool isTerminal = false;


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
        player = GameObject.Find("Player");
        Player scPlayer = player.GetComponent<Player>();
        gameManager = GameManager.Instance;
        if (PlayerPrefs.HasKey("Rifle"))
        {
            scPlayer.ChangeGun(Player.typeGun.Rifle);
            gameManager.SetGunInfor(objRifle, false);

        }
        else
        {
            scPlayer.ChangeGun(Player.typeGun.Pistol);
            gameManager.SetGunInfor(objPistol, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveWall();
    }

    private void moveWall()
    {
        if (isTerminal == true)
        {
            float wallX = wall.transform.position.x;

            if (wallX > maxX)
            {
                wallX = maxX;
                //Destroy(wall);
            }

            //���� ���� �̵��ϸ鼭 ���� ������
            wallX += Time.deltaTime;
            wall.transform.position = new Vector3(wallX, wall.transform.position.y, wall.transform.position.z);
        }
    }

    public void CheckTerminal(bool _terminal)
    {
        isTerminal = _terminal;
    }
}
