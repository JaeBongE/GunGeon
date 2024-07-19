using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] GameObject UI;
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();

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

    Transform trsBullet;

    [Header("패턴1")]
    [SerializeField] GameObject enemyBullet;
    [SerializeField] List<Transform> listPattern1 = new List<Transform>();
    private float bulletSpeed = 10f;

    [Header("패턴2")]
    [SerializeField] GameObject objPattern2;

    [Header("패턴3")]
    [SerializeField] Transform trsMuzzle;
    WaitForSeconds coolTimePattern3 = new WaitForSeconds(0.4f);

    public enum BossPattern
    {
        P1,
        P2,
        P3,
    }

    public override void Awake()
    {
        base.Awake();

    }


    public override void Start()
    {
        base.Start();

        GameObject bullets = GameObject.Find("Bullets");
        trsBullet = bullets.transform;
    }

    public override void Update()
    {
        base.Update();

        checkAnim();

        checkStopTime();

        checkBossParrtern();
        coolPattern1();
        coolPattern2();
        coolPattern3();

    }

    private void checkAnim()
    {
        if (isMove == false || isStop == true)
        {
            anim.SetBool("isStop", true);
        }
        else if (isMove == true || isStop == false)
        {
            anim.SetBool("isStop", false);
        }
    }


    public override bool GetDamage(float _damage)
    {
        bool value = base.GetDamage(_damage);

        BossUI scUI = UI.GetComponent<BossUI>();
        scUI.SetBossHp(curHp, maxHp);

        if (curHp == maxHp / 2)
        {
            moveSpeed += 3f;
        }

        if (curHp < 1)
        {
            UI.SetActive(false);
        }

        return value;
    }

    //public void checkBossHp()
    //{
    //    BossUI scUI = UI.GetComponent<BossUI>();
    //    scUI.SetBossHp(curHp, maxHp);

    //    if (curHp == maxHp / 2)
    //    {
    //        moveSpeed += 3f;
    //    }

    //    if (curHp < 1)
    //    {
    //        UI.SetActive(false);
    //    }
    //}

    public override void move()
    {
        if (isStop || isDeath) return;

        Vector3 Pos = gameObject.transform.position;
        Vector3 scale = gameObject.transform.localScale;
        base.move();

        if (isCheckPlayer == false)
        {
            if (Pos.x > AstarRandomPos.x)
            {
                scale.x = 1f;
            }
            else
            {
                scale.x = -1f;
            }
            gameObject.transform.localScale = scale;
        }
        else
        {
            if (Pos.x > targetPos.x)
            {
                scale.x = 1f;
            }
            else
            {
                scale.x = -1f;
            }
            gameObject.transform.localScale = scale;
        }
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
        if (isDeath == true) return;

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
            else
            {
                return;
            }

            switch ((BossPattern)curPattern)
            {
                case BossPattern.P1:
                    if (isCoolParrtern1 == false)
                    {
                        pattern1();
                        patternTimer = patternLimitTime;
                        pattern1ChangeTimer = pattern1ChangeLimitTime;
                        isCoolParrtern1 = true;
                    }
                    break;

                case BossPattern.P2:
                    if (isCoolParrtern2 == false)
                    {
                        pattern2();
                        patternTimer = patternLimitTime;
                        pattern2ChangeTimer = pattern2ChangeLimitTime;
                        isCoolParrtern2 = true;
                    }
                    break;

                case BossPattern.P3:
                    if (isCoolParrtern3 == false)
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
        anim.SetTrigger("pattern1");

        Debug.Log("패턴1");
        auido.PlayOneShot(audioClips[0]);

        for (int iNum = 0; iNum < listPattern1.Count; iNum++)
        {
            GameObject obj = Instantiate(enemyBullet, listPattern1[iNum].position, Quaternion.identity, trsBullet);
            GameObject obj1 = Instantiate(enemyBullet, listPattern1[iNum].position, Quaternion.identity, trsBullet);
            GameObject obj2 = Instantiate(enemyBullet, listPattern1[iNum].position, Quaternion.identity, trsBullet);
            GameObject obj3 = Instantiate(enemyBullet, listPattern1[iNum].position, Quaternion.identity, trsBullet);

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
        anim.SetTrigger("pattern2");

        Debug.Log("패턴2");
        Pattern2 scPattern2 = objPattern2.GetComponent<Pattern2>();
        scPattern2.DoPattern2();
    }

    private void pattern3()
    {
        isStop = true;

        Debug.Log("패턴3");
        auido.PlayOneShot(audioClips[2]);
        StartCoroutine(shootPattern3(trsBullet));
    }

    IEnumerator shootPattern3(Transform _trsBullets)
    {
        int count = 10;
        for (int iNum = 0; iNum < count; ++iNum)
        {
            GameObject obj = Instantiate(enemyBullet, trsMuzzle.position, Quaternion.identity, _trsBullets);
            Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();

            Vector3 shootDir = (trsPlayer.position - trsMuzzle.position).normalized;
            rigid.velocity = shootDir * bulletSpeed;

            yield return coolTimePattern3;
        }
    }

}
