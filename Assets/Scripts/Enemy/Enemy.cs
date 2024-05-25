using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    GameManager gameManager;
    public static Enemy Instance;

    [Header("�⺻����")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float curHp;
    protected Animator anim;
    [SerializeField] protected GameObject hitBox;
    protected bool isDeath = false;

    [Header("���� �̵�����")]
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

    protected SpriteRenderer spr;

    float invTimer = 0.0f;//������ �޾Ҵ���, 1���Ŀ� �ٽ� ������ ������ �ִ� ���·� �����
    [SerializeField]
    protected float InvTime = 1.0f;


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
        spr = GetComponent<SpriteRenderer>();
        isMove = true;
    }

    public virtual void Start()
    {
        targetPos = getRandomPos();
        gameManager = GameManager.Instance;
    }

    /// <summary>
    /// �������� �޾��� �� ü���� ���δ�
    /// </summary>
    /// <param name="_damage">���� ������</param>
    public virtual bool GetDamage(float _damage)
    {
        if (invTimer != 0.0f) return false;
        invTimer = InvTime;

        CheckPlayer();
        curHp -= _damage;

        if (curHp > 0)
        {
            hitAnim();
        }
        else
        {
            death();
        }
        return true;
        //if (curHp < 1)
        //{
        //    gameObject.layer = LayerMask.NameToLayer("Nodamage");
        //    hitBox.layer = LayerMask.NameToLayer("Nodamage");
        //    moveSpeed = 0f;
        //    gameManager.CheckoutEnemy(gameObject);
        //    Destroy(gameObject, 1.5f);
        //}
    }

    protected virtual void hitAnim()
    {
        if (gameObject.name == "Boss") return;

        anim.SetTrigger("Hit");
        spr.color = Color.red;
        Invoke("returnColor", InvTime);
    }

    protected void returnColor()
    {
        spr.color = Color.white;
        hitBox.layer = LayerMask.NameToLayer("EnemyHitBox");
    }

    public virtual void Update()
    {
        checkTimers();

        move();
        moveCoolTime();
        //death();

        //if (curHp < 1)//ü���� 0�̵Ǹ� ���ӸŴ��� �迭���� �˸�
        //{
        //    gameObject.layer = LayerMask.NameToLayer("Nodamage");
        //    hitBox.layer = LayerMask.NameToLayer("Nodamage");
        //    moveSpeed = 0f;
        //    gameManager.CheckoutEnemy(gameObject);
        //    Destroy(gameObject, 1.5f);
        //}
    }

    private void checkTimers()
    {
        if (invTimer != 0.0f)
        {
            invTimer -= Time.deltaTime;
            if (invTimer < 0.0f)
            {
                invTimer = 0.0f;
            }
        }
    }

    /// <summary>
    /// ������ ��ġ�� �̵��ϴٰ� Player�� �߰��ϸ� �÷��̾ ���󰡵��� ����
    /// </summary>
    public virtual void move()
    {
        if (isMove == false || isDeath == true) return;//�������ٰ� �� �� ���� �����̸� ����

        if (isCheckPlayer == false)//Player �ν� ��
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(gameObject.transform.position, targetPos) < 0.1f)//Ÿ����ġ�� �̵��Ϸ�� ���߰� ���� Ÿ����ġ�� �˻�
            {
                isMove = false;
                targetPos = getRandomPos();
            }
        }
        else//Player �ν� ��
        {
            GameObject objPlayer = GameObject.Find("Player");
            targetPos = objPlayer.transform.position;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// ������ ������ �Լ�
    /// </summary>
    private void moveCoolTime()
    {
        //Player�� �ν��ϸ� ������x
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
    /// ������ Ÿ����ġ ����
    /// </summary>
    /// <returns></returns>
    private Vector3 getRandomPos()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector3(randomX, randomY, gameObject.transform.position.z);
    }

    /// <summary>
    /// �÷��̾ �߰��ߴ��� Ȯ��
    /// </summary>
    public void CheckPlayer()
    {
        isCheckPlayer = true;
        isMove = true;
    }

    public virtual void death()
    {
        isDeath = true;
        isMove = false;
        gameObject.layer = LayerMask.NameToLayer("Nodamage");
        hitBox.layer = LayerMask.NameToLayer("Nodamage");
        moveSpeed = 0f;
        gameManager.CheckoutEnemy(gameObject);
        anim.SetTrigger("Death");

    }

    public void doDestroy()
    {
        Destroy(gameObject);
    }
}
