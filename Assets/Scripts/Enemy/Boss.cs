using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] GameObject UI;

    [Header("패턴")]
    [SerializeField] private float patternTimer = 5f;
    private float patternLimitTime = 5f;
    private int curPattern = -1;

    private float pattern1ChangeTimer = 10f;
    private float pattern1ChangeLimitTime = 10f;
    private bool isCoolParrtern1 = false;

    private float pattern2ChangeTimer = 10f;
    private float pattern2ChangeLimitTime = 10f;
    private bool isCoolParrtern2 = false;

    private float pattern3ChangeTimer = 10f;
    private float pattern3ChangeLimitTime = 10f;
    private bool isCoolParrtern3 = false;

    private float stopTimer = 2f;
    private float stopLimitTimer = 2f;
    private bool isStop = false;

    [Header("패턴1")]
    [SerializeField] GameObject enemyBullet;
    [SerializeField] List<Transform> listPattern1 = new List<Transform>();
    private float bulletSpeed = 5f;

    [Header("패턴2")]
    [SerializeField] GameObject objPattern2;


    public enum BossPattern
    {
        P1,
        P2,
        P3,
    }

    public override void Start()
    {
        base.Start();

    }

    public override void Update()
    {
        base.Update();

        checkStopTime();

        checkBossParrtern();
        coolPattern1();
        coolPattern2();
        coolPattern3();
    }

    public override bool GetDamage(float _damage)
    {
        BossUI scUI = UI.GetComponent<BossUI>();
        scUI.SetBossHp(curHp, maxHp);
        return base.GetDamage(_damage);
    }

    public override void move()
    {
        if (isStop) return;

        base.move();
    }

    private void coolPattern1()
    {
        if (pattern1ChangeTimer > 0f)
        {
            pattern1ChangeTimer -= Time.deltaTime;
            if (pattern1ChangeTimer < 0f)
            {
                pattern1ChangeTimer = 0f;
                isCoolParrtern1 = false;
            }
        }
    }
    
    private void coolPattern2()
    {
        if (pattern2ChangeTimer > 0f)
        {
            pattern2ChangeTimer -= Time.deltaTime;
            if (pattern2ChangeTimer < 0f)
            {
                pattern2ChangeTimer = 0f;
                isCoolParrtern2 = false;
            }
        }
    }
    
    private void coolPattern3()
    {
        if (pattern3ChangeTimer > 0f)
        {
            pattern3ChangeTimer -= Time.deltaTime;
            if (pattern3ChangeTimer < 0f)
            {
                pattern3ChangeTimer = 0f;
                isCoolParrtern3 = false;
            }
        }
    }


    private void checkBossParrtern()
    {
        if (isCheckPlayer)
        {
            int beforePattern = curPattern;
            while (beforePattern == curPattern)
            {
                curPattern = Random.Range(0, System.Enum.GetValues(typeof(BossPattern)).Length);
            }

            patternTimer -= Time.deltaTime;
            if (patternTimer <= 0f)
            {
                patternTimer = 0f;
            }

            switch ((BossPattern)curPattern)
            {
                case BossPattern.P1:
                    if (patternTimer == 0f && isCoolParrtern1 == false)
                    {
                        pattern1();
                        patternTimer = patternLimitTime;
                        pattern1ChangeTimer = pattern1ChangeLimitTime;
                        isCoolParrtern1 = true;
                    }
                    break;

                case BossPattern.P2:
                    if (patternTimer == 0f && isCoolParrtern2 == false)
                    {
                        pattern2();
                        patternTimer = patternLimitTime;
                        pattern2ChangeTimer = pattern2ChangeLimitTime;
                        isCoolParrtern2 = true;
                    }
                    break;

                case BossPattern.P3:
                    if (patternTimer == 0f && isCoolParrtern3 == false)
                    {
                        pattern3();
                        patternTimer = patternLimitTime;
                        pattern3ChangeTimer = pattern3ChangeLimitTime;
                        isCoolParrtern3 = true;
                    }
                    break;
            }
        }
    }

    private void checkStopTime()
    {
        if (stopTimer > 0f && isStop == true)
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer < 0f)
            {
                stopTimer = stopLimitTimer;
                isStop = false;
            }
        }
    }

    private void pattern1()
    {
        isStop = true;

        Debug.Log("패턴1");

        GameObject bullets = GameObject.Find("Bullets");
        Transform trsBullets = bullets.transform;


        for (int iNum = 0; iNum < listPattern1.Count; iNum++)
        {
            GameObject obj = Instantiate(enemyBullet, listPattern1[iNum].position, Quaternion.identity, trsBullets);
            GameObject obj1 = Instantiate(enemyBullet, listPattern1[iNum].position, Quaternion.identity, trsBullets);
            GameObject obj2 = Instantiate(enemyBullet, listPattern1[iNum].position, Quaternion.identity, trsBullets);
            GameObject obj3 = Instantiate(enemyBullet, listPattern1[iNum].position, Quaternion.identity, trsBullets);

            Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
            Rigidbody2D rigid1 = obj1.GetComponent<Rigidbody2D>();
            Rigidbody2D rigid2 = obj2.GetComponent<Rigidbody2D>();
            Rigidbody2D rigid3 = obj3.GetComponent<Rigidbody2D>();

            rigid.velocity = Vector3.up * bulletSpeed;
            rigid1.velocity = Vector3.down * bulletSpeed;
            rigid2.velocity = Vector3.left * bulletSpeed;
            rigid3.velocity = Vector3.right * bulletSpeed;
        }

        
    }

    private void pattern2()
    {
        isStop = true;

        Debug.Log("패턴2");
        Pattern2 scPattern2 = objPattern2.GetComponent<Pattern2>();
        scPattern2.DoPattern2();
    }

    private void pattern3()
    {
        isStop = true;

        Debug.Log("패턴3");
    }
}
