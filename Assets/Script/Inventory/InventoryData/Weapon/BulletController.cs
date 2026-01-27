using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public enum TypeBullet
{
    PLAYER_BULLET,
    ENEMY_BULLET
}
public class BulletController : MonoBehaviour, IPoolable
{


    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float exist_time = 2f;

    [SerializeField] private float damaged;

    private int indexBullet;

    [SerializeField] private TypeBullet bulletType;

    [SerializeField] private bool haveAnimDir;

    [SerializeField] private GameObject glow;

    [SerializeField] private bool haveAnimExplo;

    private Animator anim;



    public void OnSpawn()
    {

    }
    public void OnDeSpawn()
    {

    }
    private Rigidbody2D rb;

    public void SetInfo(float _damaged, int _indexBullet)
    {
        this.damaged = _damaged;
        this.indexBullet = _indexBullet;
    }
    public void SetSpeed(float _speed)
    {
        moveSpeed = _speed;
    }
    public void DeSpawn()
    {
        GameManageMent.Instance.PoolManager.BulletPoolsList[indexBullet].DeSpawn(this);
    }
    //Kiem tra va cham voi dich
    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (bulletType)
        {
            case TypeBullet.PLAYER_BULLET:
                {
                    if (collision.tag == GameConfig.HITBOX_ENEMY)
                    {
                        if (collision.gameObject != null)
                        {
                            bool isCrit = GameManageMent.Instance.PlayerManager.CalculateCritDamage(ref damaged);
                            collision.gameObject.GetComponentInParent<HealthEnemy>()?.OnDamaged(damaged, isCrit);
                        }
                        rb.linearVelocity = Vector2.zero;
                        if (haveAnimExplo)
                        {
                            anim.SetTrigger("Explo");
                        }

                        if (glow != null)
                        {
                            glow?.SetActive(true);
                        }
                        
                        DeSpawn();


                    }
                    else if (collision.tag == GameConfig.HITBOX_BOSS)
                    {
                        if (collision.gameObject != null)
                        {
                            bool isCrit = GameManageMent.Instance.PlayerManager.CalculateCritDamage(ref damaged);
                            collision.gameObject.GetComponentInParent<BossStat>()?.TakeDamage(damaged, isCrit);
                        }
                        rb.linearVelocity = Vector2.zero;
                        if (haveAnimExplo)
                        {
                            anim.SetTrigger("Explo");
                        }
                        if (glow != null)
                        {
                            glow?.SetActive(true);
                        }
                        
                        DeSpawn();
                        
                    }
                    break;
                }
            case TypeBullet.ENEMY_BULLET:
                if (collision.tag == GameConfig.HITBOX_PLAYER)
                {
                    Debug.Log("colliderr player");
                    if (collision.gameObject != null)
                    {
                        collision.gameObject.GetComponentInParent<Health>()?.OnDamaged(damaged);
                    }
                    rb.linearVelocity = Vector2.zero;
                    //anim.SetTrigger("Explo");
                    if (glow != null)
                    {
                        glow?.SetActive(true);
                    }
                    
                    DeSpawn();
                    
                }
                break;
            default:
                break;

        }
        if (collision.tag == GameConfig.HITBOX_DESTROYOBJECT_TAG)
        {

            // if(collision.gameObject != null)
            // {
            //     collision.gameObject.GetComponentInParent<ObjectController>()?.OnDamaged(damaged);

            // }
            rb.linearVelocity = Vector2.zero;
            if (haveAnimExplo)
            {
                anim.SetTrigger("Explo");
            }
            if (glow != null)
            {
                glow?.SetActive(true);
            }
            DeSpawn();
            

        }
        if (collision.tag == GameConfig.BLOCK_OBJECT_TAG)
        {

            rb.linearVelocity = Vector2.zero;
            if (haveAnimExplo)
            {
                anim.SetTrigger("Explo");
            }
            if (glow != null)
            {
                glow?.SetActive(true);
            }
            
            DeSpawn();
            

        }

    }
    private void UpdateAnimDir(Vector2 dir)
    {
        dir = dir.normalized;
        anim.SetFloat("DirX", dir.x);
        anim.SetFloat("DirY", dir.y);
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    IEnumerator AutoDestroy()
    {

        yield return new WaitForSeconds(exist_time);
        GameManageMent.Instance.PoolManager.BulletPoolsList[indexBullet].DeSpawn(this);
    }

    // Ban theo huong
    public void Fire(Vector2 dir)
    {

        if (glow != null)
        {
            glow?.SetActive(true);
        }


        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


        rb.linearVelocity = dir * moveSpeed;
        if (haveAnimDir)
        {
            UpdateAnimDir(dir);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        StartCoroutine(AutoDestroy());
    }
    void Start()
    {

    }
    void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
