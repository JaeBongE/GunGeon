using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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

    private void Awake()
    {
        curHp = maxHp;
        anim = GetComponent<Animator>();
        isMove = true;
    }

    private void Start()
    {
        targetPos = getRandomPos();
    }

    public virtual void GetDamage(float _damage)
    {
        anim.SetTrigger("Hit");
        curHp -= _damage;
    }

    public virtual void Update()
    {
        moveRandom();
        moveCoolTime();

        //if (curHp < 1)
        //{
        //    gameObject.layer = LayerMask.NameToLayer("Nodamage");
        //    anim.SetTrigger("Death");
        //    Destroy(gameObject, 1f);
        //}
    }

    public virtual void moveRandom()
    {
        if (isMove == false) return;

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);
        
        if (Vector3.Distance(gameObject.transform.position, targetPos) < 0.1f)
        {
            isMove = false;
            targetPos = getRandomPos();
        }


    }

    private void moveCoolTime()
    {
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
}
