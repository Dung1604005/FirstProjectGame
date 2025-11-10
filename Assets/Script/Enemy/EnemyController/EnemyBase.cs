using System;
using System.Collections;
using System.Collections.Generic;

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


        Vector2 dir = flow;
        RaycastHit2D[] hits = new RaycastHit2D[2];
        int Count = rb.Cast(dir, filter, hits, avoidDistance);

        if (Count > 0)
        {
            Vector2 avoidDir = Vector2.zero;
            for (int i = 0; i < Count; i++)
            {
                if (hits[i].distance > avoidDistance * 0.8f)
                {
                    continue;

                }
                Vector2 away = ((Vector2)transform.position - hits[i].point).normalized;
                avoidDir += away;

            }
            if (avoidDir != Vector2.zero)
            {
                avoidDir = avoidDir.normalized;
                Vector2 targetDir = (flow * wFlow + avoidDir * wAvoid).normalized;
                currentDir = Vector2.Lerp(currentDir, targetDir, 0.1f);
                dir = currentDir;
                // Debug.DrawRay(transform.position, dir * 1.0f, Color.cyan);
                // Debug.DrawRay(transform.position, targetDir, Color.yellow); ;
                // Debug.DrawRay(transform.position, avoidDir, Color.red);

            }
            else
            {
                Vector2 targetDir = flow;
                currentDir = Vector2.Lerp(currentDir, targetDir, 0.1f);
                dir = currentDir;
            }


        }
        else
        {
            Vector2 targetDir = flow;
            currentDir = Vector2.Lerp(currentDir, targetDir, 0.1f);
            dir = currentDir;
        }
        
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
            curState = State.Idle;
            return;
        }
        Vector2 flow = gridManagement.GridBuilder.GridCells[(int)gridPosition.x][(int)gridPosition.y].FlowDirection;

        if (distance > 0 && flow != Vector2.zero)
        {
            OnMove(flow);
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
