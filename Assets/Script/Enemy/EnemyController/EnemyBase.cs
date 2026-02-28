using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

    [SerializeField] protected float separationStrength;

    [SerializeField] protected float wSeparation;

    [SerializeField] protected float separationRadius;

    private Context context;



    private String[] masks = { GameConfig.OBJECT_MASK, GameConfig.BUILDING_MASK, GameConfig.PLAYER_WALL_MASK };
    private LayerMask layerMask;

    private Vector2 currentDir;

    [SerializeField] protected Rigidbody2D rb;


    private BoxCollider2D boxCollider2D;

    protected HealthEnemy healthSystem;

    public HealthEnemy HealthSystem => healthSystem;

    protected Transform player;

    protected Animator anim;

    public Animator Anim => anim;

    protected bool attacking = false;


    [SerializeField] protected EnemyBaseData enemyBaseData;

    public EnemyBaseData EnemyBaseData => enemyBaseData;

    protected float rangeChase;
    protected float attack;
    protected State curState;
    protected MoveState moveState;

    protected float cur_coolDown = 0f;
    protected int animTypeMove = 1;
    protected int animTypeAttack = 2;

    protected bool isDied = false;

    [SerializeField] private float wanderingCooldown;

    private float wanderingTimer = 0f;

    private Vector2 wanderingDir = Vector2.zero;

    [SerializeField] protected Vector2 spawnPosition;

    [SerializeField] protected float maxMoveRadius;

    [SerializeField] private bool isInPool;

    [SerializeField] private float timeSpawn;



    public float GetDamage()
    {
        return attack;
    }
    public void SetEnemyInPool()
    {
        isInPool = true;
    }
    public void SetDie()
    {
        isDied = true;
        if (isInPool)
        {
            this.gameObject.SetActive(false);
            GameManageMent.Instance.PoolManager.EnemytPoolsList[enemyBaseData.IndexEnemy].DeSpawn(this);
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            transform.position = spawnPosition;
            StartCoroutine(WaitingForSpawnRoutine());
        }
    }
    IEnumerator WaitingForSpawnRoutine()
    {
        yield return new WaitForSeconds(timeSpawn);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        SpawnEnemy(spawnPosition);
        

        GetComponent<SpriteRenderer>().enabled = true;
        
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


        float disPlayerFromSpawn = ((Vector2)player.position - spawnPosition).sqrMagnitude;
        float disEnemyFromSpawn = ((Vector2)transform.position - spawnPosition).sqrMagnitude;

        if (dis <= enemyBaseData.RangeAtk * enemyBaseData.RangeAtk && disPlayerFromSpawn <= maxMoveRadius * maxMoveRadius
        && disEnemyFromSpawn <= maxMoveRadius * maxMoveRadius)
        {
            curState = State.Attack;
        }
        else if (dis <= rangeChase * rangeChase &&
        distance < int.MaxValue / 10 && distance > 0 && disPlayerFromSpawn <= maxMoveRadius * maxMoveRadius
        && disEnemyFromSpawn <= maxMoveRadius * maxMoveRadius)
        {
            curState = State.Chase;
        }
        //Wandering
        wanderingTimer += Time.deltaTime;
        if (wanderingTimer > wanderingCooldown)
        {
            wanderingDir = UnityEngine.Random.insideUnitCircle.normalized;

            wanderingTimer = 0f;
        }
        if (disEnemyFromSpawn > maxMoveRadius * maxMoveRadius)
        {
            wanderingDir = (spawnPosition - (Vector2)transform.position).normalized;

        }

        OnMove(wanderingDir, enemyBaseData.WalkSpeed);


    }

    // Update animation chuyen dong va tan cong
    protected virtual void AnimMove(int type, float x, float y)
    {
        anim.SetFloat("MoveX", x);
        anim.SetFloat("MoveY", y);

    }

    // Tinh toan de cho enemy di chuyen den player
    protected virtual void OnMove(Vector2 flow, float speed)
    {
        if (attacking)
        {
            return;
        }


        Vector2 dir = flow.normalized;
        Vector2 pos = rb.position;
        for (int i = 0; i < context.Interest.Length; i++)
        {
            context.SetInterestElement(i, Mathf.Max(0, wFlow * Vector2.Dot(context.Dirs[i].normalized, flow)));

            context.SetDangerElement(i, 0);
        }
        RaycastHit2D hit;

        for (int i = 0; i < context.Directions; i++)
        {
            hit = Physics2D.BoxCast(pos + context.Dirs[i], new Vector2(boxCollider2D.size.x + 0.1f, boxCollider2D.size.y + 0.1f), 0, context.Dirs[i].normalized, avoidDistance, layerMask);
            Color c = Color.red;
            if (hit.collider != null)
            {
                float t = 1f - Mathf.Clamp01(hit.distance / avoidDistance);
                context.SetDangerElement(i, Mathf.Max(context.Danger[i], wAvoid * t));
            }
            else
            {
                c = Color.green;
            }
            Debug.DrawRay(pos + context.Dirs[i], context.Dirs[i] * avoidDistance, c);
        }
        float bestScore = float.NegativeInfinity;
        Vector2 bestDir = flow;
        for (int i = 0; i < context.Directions; i++)
        {
            float score = Mathf.Clamp01(context.Interest[i] - context.Danger[i]);
            if (score > bestScore)
            {
                bestScore = score;
                bestDir = context.Dirs[i];
            }
        }
        // Tach dan
        LayerMask enemyMask = LayerMask.GetMask(GameConfig.BODY_ENEMY_MASK);

        Vector2 separationForce = Vector2.zero;

        Collider2D[] collider = Physics2D.OverlapCircleAll(pos, separationRadius, enemyMask);
        int enemyCount = 0;
        foreach (Collider2D enemy in collider)
        {
            if (enemy != boxCollider2D)
            {
                enemyCount++;
                Debug.Log("YES");
                float distanceToZombie = Vector2.Distance(enemy.transform.position, transform.position);
                separationForce += (1f - Mathf.Clamp01(distanceToZombie / separationRadius)) * separationStrength * ((Vector2)(transform.position - enemy.transform.position)).normalized;

            }
        }
        if (enemyCount > 0)
        {
            separationForce /= Mathf.Sqrt(enemyCount);
        }
        bestDir = bestDir + separationForce.normalized * wSeparation;
        bestDir = bestDir.normalized;
        currentDir = Vector2.Lerp(currentDir, bestDir, 0.1f);
        Vector2 moveDir = currentDir.normalized;
        dir = moveDir;

        Debug.DrawRay(pos, moveDir * avoidDistance, Color.yellow); // hướng chọn cuối

        AnimMove(animTypeMove, dir.x, dir.y);
        Vector2 movePos = rb.position + dir * speed * Time.fixedDeltaTime;
        rb.MovePosition(movePos);
    }

    // Trang thai duoi theo player
    protected virtual void OnChase()
    {
        if (gridManagement.IsUpdating)
        {
            return;
        }
        float dis = (player.position - transform.position).sqrMagnitude;
        float disPlayerFromSpawn = ((Vector2)player.position - spawnPosition).sqrMagnitude;
        float disEnemyFromSpawn = ((Vector2)transform.position - spawnPosition).sqrMagnitude;
        if (dis > rangeChase * rangeChase || disPlayerFromSpawn > maxMoveRadius * maxMoveRadius ||
         disEnemyFromSpawn > maxMoveRadius * maxMoveRadius)
        {
            curState = State.Idle;
        }
        else if (dis <= enemyBaseData.RangeAtk * enemyBaseData.RangeAtk)
        {
            curState = State.Attack;
        }
        Vector2 gridPosition = gridManagement.GridBuilder.WorldToGridPosition(transform.position);
        if (!gridManagement.GridBuilder.IsValidGridPosition(gridPosition))
        {
            curState = State.Idle;
            return;
        }
        int distance = (int)gridManagement.GridBuilder.GridCells[(int)gridPosition.x][(int)gridPosition.y].DistanceFromPlayer;
        if (distance >= int.MaxValue / 10)
        {
            //Debug.Log("Here");
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
            OnMove(randomDir, enemyBaseData.RunSpeed);
            return;
        }
        Vector2 flow = gridManagement.GridBuilder.GridCells[(int)gridPosition.x][(int)gridPosition.y].FlowDirection;

        if (distance > 0 && flow != Vector2.zero)
        {
            OnMove(flow, enemyBaseData.RunSpeed);
        }
        else
        {
            //Debug.Log("Here");
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
            OnMove(randomDir, enemyBaseData.RunSpeed);
        }



    }
    public void SpawnEnemy(Vector3 pos)
    {
        transform.position = pos;

        curState = State.Idle;
        cur_coolDown = 0f;
        currentDir = Vector2.zero;
        isDied = false;
        attacking = false;
        healthSystem.SetCurHealth(enemyBaseData.MaxHealth);
        
        // Reset thanh mau
        
        this.transform.GetChild(1).gameObject.SetActive(false);
        this.transform.GetChild(2).gameObject.SetActive(false);
    }
    public void SetAttack(float atk)
    {
        attack = atk;
    }
    public void SetRangeChase(float _rangeChase)
    {
        rangeChase = _rangeChase;
    }

    // Trang thai tan cong 
    protected virtual void OnAttack()
    {

    }

    void Start()
    {


        Init();
        if (GameObject.FindWithTag(GameConfig.PLAYER_TAG0) != null)
        {

            player = GameObject.FindWithTag(GameConfig.PLAYER_TAG0).transform;

        }


    }
    protected virtual void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        curState = State.Idle;
        anim = GetComponent<Animator>();
        cur_coolDown = 0f;
        isDied = false;
        healthSystem = GetComponent<HealthEnemy>();
        healthSystem.SetMaxHealth(enemyBaseData.MaxHealth);
        healthSystem.SetCurHealth(enemyBaseData.MaxHealth);
        layerMask = LayerMask.GetMask(masks);
        currentDir = Vector2.zero;
        context = new Context();
        boxCollider2D = GetComponent<BoxCollider2D>();
        gridManagement = GameManageMent.Instance.GridManagement;
        SetAttack(enemyBaseData.Atk);
        SetRangeChase(enemyBaseData.RangeChase);
        spawnPosition = this.transform.position;
    }
    protected virtual void Awake()
    {

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
        if (player == null) return;
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
