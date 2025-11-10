using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public enum State
{
    Idle, Attack, Chase
}

public enum MoveState
{
    Left, Right, Up, Down,
}
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected GridManagement gridManagement;

    [SerializeField] protected float wFlow;

    [SerializeField] protected float wAvoid;

    [SerializeField] protected float avoidDistance;

    private Context context;

    [SerializeField] float whiskerAngle = 35f;

    [SerializeField] int whiskerCount = 5; // 3: center, left, right
    [SerializeField] float wAlign = 0.7f;
    [SerializeField] float wClear = 1.3f;
    [SerializeField] float turnLerp = 0.15f;

    private String[] masks = { GameConfig.OBJECT_MASK, GameConfig.BUILDING_MASK, GameConfig.PLAYER_WALL_MASK };
    private LayerMask layerMask;

    private Vector2 currentDir;

    [SerializeField] protected Rigidbody2D rb;

    private ContactFilter2D filter;

    protected HealthEnemy healthSystem;

    public HealthEnemy HealthSystem => healthSystem;

    protected Transform player;

    protected Animator anim;

    protected bool attacking = false;


    [SerializeField] protected EnemyBaseData enemyBaseData;

    public EnemyBaseData EnemyBaseData => enemyBaseData;
    protected State curState;
    protected MoveState moveState;

    protected float cur_coolDown = 0f;
    protected int animTypeMove = 1;
    protected int animTypeAttack = 2;

    protected bool isDied = false;



    public float GetDamage()
    {
        return enemyBaseData.Atk;
    }
    public void SetDie()
    {
        isDied = true;

    }
    // State dung yen
    protected virtual void OnIdle()
    {
        float dis = (player.position - transform.position).sqrMagnitude;

        Vector2 gridPosition = gridManagement.GridBuilder.WorldToGridPosition(transform.position);
        if (!gridManagement.GridBuilder.IsValidGridPosition(gridPosition))
        {
            return;
        }
        Vector2 flow = gridManagement.GridBuilder.GridCells[(int)gridPosition.x][(int)gridPosition.y].FlowDirection;
        int distance = (int)gridManagement.GridBuilder.GridCells[(int)gridPosition.x][(int)gridPosition.y].DistanceFromPlayer;


        if (dis <= enemyBaseData.RangeAtk * enemyBaseData.RangeAtk)
        {
            curState = State.Attack;
        }
        else if (dis <= enemyBaseData.RangeChase * enemyBaseData.RangeChase &&
        distance < int.MaxValue / 10 && distance > 0)
        {
            curState = State.Chase;
        }

    }

    // Update animation chuyen dong va tan cong
    protected virtual void AnimMove(int type, float x, float y)
    {
        anim.SetFloat("MoveX", x);
        anim.SetFloat("MoveY", y);

    }

    // Tinh toan de cho enemy di chuyen den player
    protected virtual void OnMove(Vector2 flow)
    {
        Vector2 dir = flow.normalized;
        Vector2[] candidates = new Vector2[whiskerCount];
        int mid = whiskerCount / 2;
        candidates[mid] = flow;

        // nếu 5 whisker -> index 0..4, mid = 2
        for (int i = 1; i <= mid; i++)
        {

            float angle = i * (whiskerAngle / mid);
            float rad = angle * Mathf.Deg2Rad;

            // quay trái (+angle)
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            Vector2 left = new Vector2(
                flow.x * cos - flow.y * sin,
                flow.x * sin + flow.y * cos
            );

            // quay phải (-angle)
            Vector2 right = new Vector2(
                flow.x * cos + flow.y * sin,
                -flow.x * sin + flow.y * cos
            );

            candidates[mid - i] = left;
            candidates[mid + i] = right;

        }
        for (int i = 0; i < candidates.Length; i++)
        {

            Debug.DrawRay(transform.position, candidates[i]);
        }
        RaycastHit2D[] hits = new RaycastHit2D[4];
        float bestScore = float.NegativeInfinity;
        Vector2 bestDir = flow;
        RaycastHit2D bestHit = new RaycastHit2D();
        int hitCount = 0;

        for (int i = 0; i < whiskerCount; i++)
        {
            Vector2 canDir = candidates[i].normalized;
            RaycastHit2D minRayCastHit = hits[0];
            if (canDir == Vector2.zero) continue;
            
            int Count = rb.Cast(canDir, filter, hits, avoidDistance);
            float minHit = avoidDistance;
            hitCount += Count;


            for (int j = 0; j < Count; j++)
            {
                var hit = hits[j];


                if (hit.distance < minHit)
                {
                    minHit = hit.distance;
                    minRayCastHit = hit;

                }
            }
            float clear = Mathf.Clamp01(minHit / avoidDistance);
            float align = Vector2.Dot(canDir.normalized, flow.normalized);

            if (wClear * clear + wAlign * align >= bestScore)
            {

                bestScore = wClear * clear + wAlign * align;
                bestDir = canDir;
                if (minHit != avoidDistance)
                {
                    bestHit = minRayCastHit;
                }

            }
            if(i == 4)
            {
                Debug.Log(wClear * clear + wAlign * align);
            }

        }
        

        if (bestScore < 0.6f && hitCount > 0)
        {

            Vector2 normal = hits[0].normal.normalized;
            if (!bestHit.IsUnityNull())
            {
                normal = bestHit.normal.normalized;

            }



            // Hướng trượt dọc tường (vuông góc normal)
            Vector2 tangent = new Vector2(-normal.y, normal.x);


            // Nếu tangent đi ngược flow -> đảo lại
            if (Vector2.Dot(tangent, flow) < 0)
            {
                tangent = -tangent;
            }
            else
            {
                if (Vector2.Dot(tangent, flow) == 0 && new Vector2(-tangent.y, -tangent.x) == flow)
                {
                    tangent = -tangent;
                }
            }
            //Debug.Log(tangent + " " + flow);
            // (0, -1)
            bestDir = tangent; // đi dọc tường thay vì quay đầu

        }
        

        // int Count = rb.Cast(dir, filter, hits, avoidDistance);

        // if (Count > 0)
        // {
        //     Vector2 avoidDir = Vector2.zero;
        //     for (int i = 0; i < Count; i++)
        //     {
        //         if (Vector2.Dot(hits[i].normal, dir) > -0.1f) 
        //             continue;
        //         if (hits[i].distance > avoidDistance * 0.8f)
        //         {
        //             continue;

        //         }
        //         Vector2 away = (Vector2) transform.position - hits[i].point;
        //         avoidDir += away.normalized;

        //     }
        //     if (avoidDir != Vector2.zero)
        //     {
        //         avoidDir = avoidDir.normalized;
        //         Vector2 targetDir = (flow * wFlow + avoidDir * wAvoid).normalized;
        //         currentDir = Vector2.Lerp(currentDir, targetDir, 0.1f);
        //         dir = currentDir.normalized;
        //         Debug.DrawRay(transform.position, dir * 1.0f, Color.cyan);
        //         Debug.DrawRay(transform.position, targetDir, Color.yellow); ;
        //         Debug.DrawRay(transform.position, avoidDir, Color.red);

        //     }
        //     else
        //     {
        //         Vector2 targetDir = flow;
        //         currentDir = Vector2.Lerp(currentDir, targetDir, 0.1f);
        //         dir = currentDir.normalized;
        //     }


        // }
        // else
        // {
        //     Vector2 targetDir = flow;
        //     currentDir = Vector2.Lerp(currentDir, targetDir, 0.1f);
        //     dir = currentDir.normalized;
        // }
        currentDir = Vector2.Lerp(currentDir, bestDir, 0.1f);
        dir = currentDir;



        AnimMove(animTypeMove, dir.x, dir.y);
        Vector2 movePos = rb.position + dir * enemyBaseData.Speed * Time.fixedDeltaTime;
        rb.MovePosition(movePos);
    }

    // Trang thai duoi theo player
    protected virtual void OnChase()
    {

        float dis = (player.position - transform.position).sqrMagnitude;
        if (dis > enemyBaseData.RangeChase * enemyBaseData.RangeChase)
        {
            curState = State.Idle;
        }
        else if (dis <= enemyBaseData.RangeAtk * enemyBaseData.RangeAtk)
        {
            curState = State.Attack;
        }
        Vector2 gridPosition = gridManagement.GridBuilder.WorldToGridPosition(transform.position);
        int distance = (int)gridManagement.GridBuilder.GridCells[(int)gridPosition.x][(int)gridPosition.y].DistanceFromPlayer;
        if (distance >= int.MaxValue / 10)
        {
            //Debug.Log("Here");
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
            OnMove(randomDir);
            return;
        }
        Vector2 flow = gridManagement.GridBuilder.GridCells[(int)gridPosition.x][(int)gridPosition.y].FlowDirection;

        if (distance > 0 && flow != Vector2.zero)
        {
            OnMove(flow);
        }
        else
        {
            //Debug.Log("Here");
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
            OnMove(randomDir);
        }



    }
    public void SpawnEnemy(Vector3 pos)
    {
        transform.position = pos;
        curState = State.Idle;
        isDied = false;
        healthSystem.SetCurHealth(enemyBaseData.MaxHealth);
        this.gameObject.SetActive(true);
        // Reset thanh mau
        healthSystem.SetCurHealth(enemyBaseData.MaxHealth);
        float scale = healthSystem.CurHealth / healthSystem.MaxHealth;
        this.transform.GetChild(1).gameObject.SetActive(false);
        this.transform.GetChild(2).gameObject.SetActive(false);
    }

    // Trang thai tan cong 
    protected virtual void OnAttack()
    {

    }

    void Start()
    {



        if (GameObject.FindWithTag(GameConfig.PLAYER_TAG0) != null)
        {

            player = GameObject.FindWithTag(GameConfig.PLAYER_TAG0).transform;

        }


    }
    protected virtual void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        curState = State.Idle;
        anim = GetComponent<Animator>();
        cur_coolDown = 0f;
        healthSystem = GetComponent<HealthEnemy>();
        healthSystem.SetMaxHealth(enemyBaseData.MaxHealth);
        healthSystem.SetCurHealth(enemyBaseData.MaxHealth);
        filter = new ContactFilter2D();
        layerMask = LayerMask.GetMask(masks);
        filter.SetLayerMask(layerMask);
        currentDir = Vector2.left;
        context = new Context();



    }
    protected void Update()
    {
        if (isDied)
        {
            return;
        }
        if (GameManageMent.Instance.GameState == GameState.Pause)
        {
            return;
        }
        if (attacking == false && cur_coolDown < enemyBaseData.CoolDown)
        {
            cur_coolDown += Time.deltaTime;
        }

    }
    // Chuyen doi giua cac trang thai
    protected virtual void FixedUpdate()
    {
        if (GameManageMent.Instance.GameState == GameState.Pause)
        {
            return;
        }
        if (!isDied)
        {

            switch (curState)
            {
                case State.Idle:
                    //Debug.Log("Idle");
                    OnIdle();
                    break;
                case State.Chase:
                    //Debug.Log("CHASE");
                    OnChase();
                    break;
                case State.Attack:
                    //Debug.Log("ATTACK");
                    OnAttack();
                    break;

            }

        }


    }
}
