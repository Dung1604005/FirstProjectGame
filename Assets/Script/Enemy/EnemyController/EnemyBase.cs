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

    protected Health healthSystem;
    

    protected Transform player;

    protected Animator anim;

    protected bool attacking = false;


    [SerializeField] protected EnemyBaseData enemyBaseData;
    protected State curState;
    protected MoveState moveState;

    protected float cur_coolDown = 0f;
    protected int animTypeMove = 1;
    protected int animTypeAttack = 2;

    protected bool isDied = false;
   
    public void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == GameConfig.HITBOX_PUNCH)
        {
            
            healthSystem.OnDamaged(PlayerController.Instance.Stat.Atk);
        }
    }

    public float GetDamage()
    {
        return enemyBaseData.Atk;
    }
    public void SetDie()
    {
        isDied = true;
        PlayerController.Instance.ExpSystem.GainExp(enemyBaseData.ExpValue);
    }
    // State dung yen
    protected virtual void OnIdle()
    {
        float dis = (player.position - transform.position).sqrMagnitude;

        if (dis <= enemyBaseData.RangeAtk * enemyBaseData.RangeAtk)
        {
            curState = State.Attack;
        }
        else if (dis <= enemyBaseData.RangeChase * enemyBaseData.RangeChase)
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
        Vector2 movePos = rb.position + dir * enemyBaseData.Speed * Time.fixedDeltaTime;
        rb.MovePosition(movePos);
    }

    // Trang thai duoi theo player
    protected virtual void OnChase()
    {

        float dis = (player.position - transform.position).sqrMagnitude;
        OnMove();
        if (dis > enemyBaseData.RangeChase * enemyBaseData.RangeChase)
        {
            curState = State.Idle;
        }
        else if (dis <= enemyBaseData.RangeAtk * enemyBaseData.RangeAtk)
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
        cur_coolDown = 0f;
        healthSystem = GetComponent<Health>();
        healthSystem.SetMaxHp(enemyBaseData.MaxHealth);
        healthSystem.SetCurHp(enemyBaseData.MaxHealth);
        

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
