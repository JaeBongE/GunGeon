using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    GameManager gameManager;
    public static Enemy Instance;

    [Header("기본스텟")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float curHp;
    protected Animator anim;

    [Header("적의 이동범위")]
    [SerializeField] protected float maxX;
    [SerializeField] protected float minX;
    [SerializeField] protected float maxY;
    [SerializeField] protected float minY;
    [SerializeField] protected float moveSpeed;
    protected Vector3 targetPos;
    protected bool isMove = true;
    protected float moveMaxCool = 2f;
    protected float moveCool = 0f;
    [SerializeField] protected bool isCheckPlayer = false;

    public virtual void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(this);
        //}

        curHp = maxHp;
        anim = GetComponent<Animator>();
        isMove = true;
    }

    private void Start()
    {
        targetPos = getRandomPos();
        gameManager = GameManager.Instance;
    }

    /// <summary>
    /// 데미지를 받았을 때 체력이 깎인다
    /// </summary>
    /// <param name="_damage">받은 데미지</param>
    public virtual void GetDamage(float _damage)
    {
        anim.SetTrigger("Hit");
        curHp -= _damage;
    }

    public virtual void Update()
    {
        move();
        moveCoolTime();

        if (curHp < 1)//체력이 0이되면 게임매니저 배열삭제 알림
        {
            moveSpeed = 0f;
            gameManager.CheckoutEnemy(gameObject);
            Destroy(gameObject, 1f);
        }
    }

    /// <summary>
    /// 랜덤한 위치로 이동하다가 Player를 발견하면 플레이어를 따라가도록 설계
    /// </summary>
    public virtual void move()
    {
        if (isMove == false) return;//움직였다가 한 번 쉬는 딜레이를 만듬

        if (isCheckPlayer == false)//Player 인식 전
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(gameObject.transform.position, targetPos) < 0.1f)//타겟위치로 이동완료시 멈추고 다음 타겟위치를 검색
            {
                isMove = false;
                targetPos = getRandomPos();
            }
        }
        else//Player 인식 후
        {
            GameObject objPlayer = GameObject.Find("Player");
            targetPos = objPlayer.transform.position;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 움직임 딜레이 함수
    /// </summary>
    private void moveCoolTime()
    {
        //Player를 인식하면 딜레이x
        if (isCheckPlayer == true) return;

        if (isMove == false)
        {
            moveMaxCool -= Time.deltaTime;
            if (moveMaxCool < moveCool)
            {
                moveMaxCool = 2f;
                isMove = true;
            }
        }
    }

    /// <summary>
    /// 랜덤한 타겟위치 생성
    /// </summary>
    /// <returns></returns>
    private Vector3 getRandomPos()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector3(randomX, randomY, gameObject.transform.position.z);
    }

    /// <summary>
    /// 플레이어를 발견했는지 확인
    /// </summary>
    public void CheckPlayer()
    {
        isCheckPlayer = true;
        isMove = true;
    }
}
