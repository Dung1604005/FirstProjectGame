using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    

    private StatPlayer stat;
    public StatPlayer Stat => stat;

    private ExpSystem expSystem;
    public ExpSystem ExpSystem => expSystem;

    private Health health;
    public Health Health => health;
    public static PlayerController Instance { get; private set; }
    private Rigidbody2D rb;
    private bool usingGun = false;
    private float attackCountDown = 0f;

    private Animator anim;
    // Kiem soat va cham
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "AttackMeleeEnemyHitBox")
        {
            float dam = collision.gameObject.GetComponentInParent<EnemyMelee>().GetDame();
            this.GetComponent<Health>().OnDamaged(dam);
        }
        
    }

    // Tao anim cho Blend tree
    public void EquipGunAnim()
    {
        usingGun = true;
        anim.SetBool("UsingGun", usingGun);  
    }
    public void UnEquipGunAnim()
    {
        usingGun = false;
        anim.SetBool("UsingGun", usingGun);  
    }
    public void AnimUpdate(float x, float y)
    {
        anim.SetFloat("MoveX", x);
        anim.SetFloat("MoveY", y);


    }
    public Vector2 GetDirFromMouseToPlayer()
    {
        Vector2 playerPos = getPos();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = (mousePos - playerPos).normalized;
        return dir;
    }
    //Di chuyen
    void Move()
    {
        float movex = Input.GetAxis("Horizontal");
        float movey = Input.GetAxis("Vertical");
        if (weapon.Attacking == false)
        {
            if (usingGun)
            {
                weapon.UpdateAnim(movex, movey);
            }
            AnimUpdate(movex, movey);
        }
        
    
        
        Vector2 dir = new Vector2(movex, movey).normalized;
        Vector2 new_pos = rb.position + dir * Time.fixedDeltaTime * stat.Speed;
        rb.MovePosition(new_pos);
    }
    // Lay vi tri
    public Vector2 getPos()
    {
        //Debug.Log(rb.position.x + ", " + rb.position.y);
        return rb.position;
    }
    // Ban
    void Attack()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 dir = GetDirFromMouseToPlayer();
            weapon.Attack(dir.x, dir.y);
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        expSystem = GetComponent<ExpSystem>();
        stat = GetComponent<StatPlayer>();
        health = GetComponent<Health>();
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }



    }
    void Start()
    {
        health.SetMaxHp(stat.MaxHP, true);

    }
    void FixedUpdate()
    {
        if (GameManageMent.Instance.GameState == GameState.Pause)
        {
            return;
        }
        Move();

    }
    void Update()
    {
        if (GameManageMent.Instance.GameState == GameState.Pause)
        {
            return;
        }
        if (weapon.WeaponData.Type == ItemType.Gun)
        {
            EquipGunAnim();
        }
        if (weapon.Attacking == false)
        {
            attackCountDown += Time.deltaTime;
            if (attackCountDown >= weapon.WeaponData.CoolDown)
            {
                Attack();
            }
        }
        else
        {
            attackCountDown = 0f;
        }
        
        


    }
}
