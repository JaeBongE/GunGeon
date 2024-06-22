using Aoiti.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : MonoBehaviour
{
    GameManager gameManager;
    public static Enemy Instance;
    //[SerializeField] protected GameObject UI;

    [Header("기본스텟")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float curHp;
    protected Animator anim;
    [SerializeField] protected GameObject hitBox;
    protected bool isDeath = false;
    BoxCollider2D colider;

    [Header("적의 이동범위")]
    [SerializeField] protected float maxX;
    [SerializeField] protected float minX;
    [SerializeField] protected float maxY;
    [SerializeField] protected float minY;
    [SerializeField] protected float moveSpeed;
    protected Vector3 targetPos;
    protected Transform trsPlayer;
    protected bool isMove = true;
    protected float moveMaxCool = 2f;
    protected float moveCool = 0f;
    [SerializeField] protected bool isCheckPlayer = false;

    protected SpriteRenderer spr;

    float invTimer = 0.0f;//공격을 받았는지, 1초후에 다시 공격을 받을수 있는 상태로 변경됨
    [SerializeField]
    protected float InvTime = 1.0f;

    [Header("Navigator options")]
    [SerializeField] float gridSize = 0.5f; //increase patience or gridSize for larger maps
    [SerializeField] float speed = 0.05f; //increase for faster movement
    protected bool Astar = false;
    [SerializeField] protected Vector2 AstarRandomPos;

    Pathfinder<Vector2> pathfinder; //the pathfinder object that stores the methods and patience
    [Tooltip("The layers that the navigator can not pass through.")]
    [SerializeField] LayerMask obstacles;
    [Tooltip("Deactivate to make the navigator move along the grid only, except at the end when it reaches to the target point. This shortens the path but costs extra Physics2D.LineCast")]
    [SerializeField] bool searchShortcut = false;
    [Tooltip("Deactivate to make the navigator to stop at the nearest point on the grid.")]
    [SerializeField] bool snapToGrid = false;
    Vector2 targetNode; //target in 2D space
    List<Vector2> path;
    List<Vector2> pathLeftToGo = new List<Vector2>();
    [SerializeField] bool drawDebugLines;

#if UNITY_EDITOR
    [SerializeField] bool show;
    [SerializeField] float radius = 0.5f;
    [SerializeField] Color color;

    protected AudioSource auido;

    private void OnDrawGizmos()
    {
        if (show == true)
        {
            Handles.color = color;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, radius);
        }
    }

#endif

    
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
        colider = GetComponent<BoxCollider2D>();
        auido = GetComponent<AudioSource>();
    }

    public virtual void Start()
    {
        targetPos = getRandomPos();
        gameManager = GameManager.Instance;

        pathfinder = new Pathfinder<Vector2>(GetDistance, GetNeighbourNodes, 1000); //increase patience or gridSize for larger maps
    }

    void GetMoveCommand(Vector2 target)
    {
        Vector2 closestNode = GetClosestNode(transform.position);
        if (pathfinder.GenerateAstarPath(closestNode, GetClosestNode(target), out path)) //Generate path between two points on grid that are close to the transform position and the assigned target.
        {
            if (searchShortcut && path.Count > 0)
                pathLeftToGo = ShortenPath(path);
            else
            {
                pathLeftToGo = new List<Vector2>(path);
                if (!snapToGrid) pathLeftToGo.Add(target);
            }

        }

    }


    /// <summary>
    /// Finds closest point on the grid
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    Vector2 GetClosestNode(Vector2 target)
    {
        return new Vector2(Mathf.Round(target.x / gridSize) * gridSize, Mathf.Round(target.y / gridSize) * gridSize);
    }

    /// <summary>
    /// A distance approximation. 
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <returns></returns>
    float GetDistance(Vector2 A, Vector2 B)
    {
        return (A - B).sqrMagnitude; //Uses square magnitude to lessen the CPU time.
    }

    /// <summary>
    /// Finds possible conenctions and the distances to those connections on the grid.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    Dictionary<Vector2, float> GetNeighbourNodes(Vector2 pos)
    {
        Dictionary<Vector2, float> neighbours = new Dictionary<Vector2, float>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) continue;

                Vector2 dir = new Vector2(i, j) * gridSize;
                if (!Physics2D.Linecast(pos, pos + dir, obstacles))
                {
                    neighbours.Add(GetClosestNode(pos + dir), dir.magnitude);
                }
            }

        }
        return neighbours;
    }


    List<Vector2> ShortenPath(List<Vector2> path)
    {
        List<Vector2> newPath = new List<Vector2>();

        for (int i = 0; i < path.Count; i++)
        {
            newPath.Add(path[i]);
            for (int j = path.Count - 1; j > i; j--)
            {
                if (!Physics2D.Linecast(path[i], path[j], obstacles))
                {

                    i = j;
                    break;
                }
            }
            newPath.Add(path[i]);
        }
        newPath.Add(path[path.Count - 1]);
        return newPath;
    }

    /// <summary>
    /// 데미지를 받았을 때 체력이 깎인다
    /// </summary>
    /// <param name="_damage">받은 데미지</param>
    public virtual bool GetDamage(float _damage)
    {
        if (invTimer != 0.0f || isDeath == true) return false;
        invTimer = InvTime;

        CheckPlayer();
        curHp -= _damage;

        //Boss scBoss = GetComponent<Boss>();
        //scBoss.checkBossHp();

        //if (gameObject.name == "Boss")
        //{
        //    BossUI scUI = UI.GetComponent<BossUI>();
        //    scUI.SetBossHp(curHp, maxHp);

        //    if (curHp == maxHp / 2)
        //    {
        //        moveSpeed += 3f;
        //    }
        //}

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

        //if (curHp < 1)//체력이 0이되면 게임매니저 배열삭제 알림
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
    /// 랜덤한 위치로 이동하다가 Player를 발견하면 플레이어를 따라가도록 설계
    /// </summary>
    public virtual void move()
    {
        if (isMove == false || isDeath == true) return;//움직였다가 한 번 쉬는 딜레이를 만듬

        if (isCheckPlayer == false)//Player 인식 전
        {
            colider.enabled = false;

            //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);
            if (Astar == false)
            {
                while (true)
                {
                    float randomX = Random.Range(minX, maxX);
                    float randomY = Random.Range(minY, maxY);
                    AstarRandomPos = new Vector2(randomX, randomY);
                    if (Physics2D.OverlapCircleAll(AstarRandomPos, 0.5f, obstacles).Length == 0)//포지션이 wall 콜라이더에 없을때만 통과
                    {
                        break;
                    }
                }

                Astar = true;
            }

            GetMoveCommand(AstarRandomPos);

            if (pathLeftToGo.Count > 0) //if the target is not yet reached
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, (Vector3)pathLeftToGo[0], moveSpeed * Time.deltaTime);

                //Vector3 dir = (Vector3)pathLeftToGo[0] - transform.position;
                //transform.position += dir.normalized * speed;

                //if (((Vector2)transform.position - pathLeftToGo[0]).sqrMagnitude < speed)
                //{
                //    transform.position = pathLeftToGo[0];
                //    pathLeftToGo.RemoveAt(0);

                //}
            }

            if (drawDebugLines)
            {
                for (int i = 0; i < pathLeftToGo.Count - 1; i++) //visualize your path in the sceneview
                {
                    Debug.DrawLine(pathLeftToGo[i], pathLeftToGo[i + 1]);
                }
            }



            if (Vector3.Distance(gameObject.transform.position, AstarRandomPos) < 0.1f)//타겟위치로 이동완료시 멈추고 다음 타겟위치를 검색
            {
                transform.position = pathLeftToGo[0];
                pathLeftToGo.RemoveAt(0);

                isMove = false;
                //targetPos = getRandomPos();
                Astar = false;
            }

        }
        else//Player 인식 후
        {
            colider.enabled = true;

            GameObject objPlayer = GameObject.Find("Player");
            targetPos = objPlayer.transform.position;
            trsPlayer = objPlayer.transform;
            //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);

            GetMoveCommand(targetPos);

            if (pathLeftToGo.Count > 0) //if the target is not yet reached
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, (Vector3)pathLeftToGo[0], moveSpeed * Time.deltaTime);

                //Vector3 dir = (Vector3)pathLeftToGo[0] - transform.position;
                //transform.position += dir.normalized * speed;
                //if (((Vector2)transform.position - pathLeftToGo[0]).sqrMagnitude < speed)
                //{
                //    transform.position = pathLeftToGo[0];
                //    pathLeftToGo.RemoveAt(0);
                //}
            }

            if (drawDebugLines)
            {
                for (int i = 0; i < pathLeftToGo.Count - 1; i++) //visualize your path in the sceneview
                {
                    Debug.DrawLine(pathLeftToGo[i], pathLeftToGo[i + 1]);
                }
            }
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
                Astar = false;
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
        CancelInvoke("returnColor");
    }
}
