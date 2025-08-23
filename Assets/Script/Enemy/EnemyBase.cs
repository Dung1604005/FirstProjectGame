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


    [SerializeField] protected Rigidbody2D rb;

    protected Transform player;

    protected Animator anim;



    protected State curState;
    protected MoveState moveState;

    [SerializeField] protected float damaged;

    [SerializeField] protected float rangeAttack;
    [SerializeField] protected float rangeChase;

    [SerializeField] protected float coolDownAttack;

    protected float cur_coolDown = 0f;
    protected int animTypeMove = 1;
    protected int animTypeAttack = 2;

    protected bool isDied = false;

    [SerializeField] protected float speed;

    public float GetDame()
    {
        return damaged;
    }
    public void SetDie()
    {
        isDied = true;
    }
    // State dung yen
    protected virtual void OnIdle()
    {
        float dis = (player.position - transform.position).sqrMagnitude;

        if (dis <= rangeAttack * rangeAttack)
        {
            curState = State.Attack;
        }
        else if (dis <= rangeChase * rangeChase)
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
    protected virtual void OnMove()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        AnimMove(animTypeMove, dir.x, dir.y);
        Vector2 movePos = rb.position + dir * speed * Time.fixedDeltaTime;
        rb.MovePosition(movePos);
    }

    // Trang thai duoi theo player
    protected virtual void OnChase()
    {

        float dis = (player.position - transform.position).sqrMagnitude;
        OnMove();
        if (dis > rangeChase * rangeChase)
        {
            curState = State.Idle;
        }
        else if (dis <= rangeAttack * rangeAttack)
        {
            curState = State.Attack;
        }

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
        cur_coolDown = coolDownAttack;

    }
    protected void Update()
    {
        if (GameManageMent.Instance.GameState == GameState.Pause)
        {
            return;
        }
        if (cur_coolDown < coolDownAttack)
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
