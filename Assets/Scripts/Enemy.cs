using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    GameManager gameManager;
    public static Enemy Instance;

    [SerializeField] protected float maxHp;
    [SerializeField] protected float curHp;
    protected Animator anim;

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

        curHp = maxHp;
        anim = GetComponent<Animator>();
        isMove = true;
    }

    private void Start()
    {
        targetPos = getRandomPos();
        gameManager = GameManager.Instance;
    }

    public virtual void GetDamage(float _damage)
    {
        anim.SetTrigger("Hit");
        curHp -= _damage;
    }

    public virtual void Update()
    {
        move();
        moveCoolTime();

        if (curHp < 1)
        {
            moveSpeed = 0f;
            gameManager.CheckoutEnemy(gameObject);
            Destroy(gameObject, 1f);
        }
    }

    public virtual void move()
    {
        if (isMove == false) return;

        if (isCheckPlayer == false)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(gameObject.transform.position, targetPos) < 0.1f)
            {
                isMove = false;
                targetPos = getRandomPos();
            }
        }
        else
        {
            GameObject objPlayer = GameObject.Find("Player");
            targetPos = objPlayer.transform.position;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }

    private void moveCoolTime()
    {
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

    private Vector3 getRandomPos()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector3(randomX, randomY, gameObject.transform.position.z);
    }

    public void CheckPlayer()
    {
        isCheckPlayer = true;
        isMove = true;
    }
}
