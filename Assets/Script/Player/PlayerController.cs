using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private StatPlayer stat;
    public StatPlayer Stat => stat;

    private ExpSystem expSystem;
    public ExpSystem ExpSystem => expSystem;

    private Health health;
    public Health Health => health;
    public static PlayerController Instance { get; private set; }
    private Rigidbody2D rb;
    
    [SerializeField] private Pistol_Bullet pistol_;

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
    void AnimUpdate(float x, float y)
    {
        anim.SetFloat("MoveX", x);
        anim.SetFloat("MoveY", y);
       
      
    }
    //Di chuyen
    void Move()
    {
        float movex = Input.GetAxis("Horizontal");
        float movey = Input.GetAxis("Vertical");
        AnimUpdate(movex, movey);
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
    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            pistol_.FirePistol();
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
        Shoot();


    }
}
