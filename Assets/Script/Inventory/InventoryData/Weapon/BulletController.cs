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


    [SerializeField] private float moveSpeed= 5f;
    [SerializeField] private float exist_time = 2f;

    [SerializeField] private float damaged;

    private int indexBullet;

    [SerializeField] private TypeBullet bulletType;

    [SerializeField] private bool haveAnimDir;

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
        this.damaged=_damaged;
        this.indexBullet = _indexBullet;
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
                   if(collision.gameObject != null )
                   {
                      
                      collision.gameObject.GetComponentInParent<HealthEnemy>()?.OnDamaged(damaged);
                   }
                   GameManageMent.Instance.PoolManager.BulletPoolsList[indexBullet].DeSpawn(this);
                }
                break;
            }
            case TypeBullet.ENEMY_BULLET:
                if (collision.tag == GameConfig.HITBOX_PLAYER)
                {
                    Debug.Log("colliderr player");
                   if(collision.gameObject != null)
                   {
                      collision.gameObject.GetComponentInParent<Health>()?.OnDamaged(damaged);
                   }
                   GameManageMent.Instance.PoolManager.BulletPoolsList[indexBullet].DeSpawn(this);
                }
                break;
            default:
            break;

        }
        if (collision.tag == GameConfig.HITBOX_DESTROYOBJECT_TAG)
        {
            
                if(collision.gameObject != null)
                {
                    collision.gameObject.GetComponentInParent<ObjectController>()?.OnDamaged(damaged);
                    
                }
                GameManageMent.Instance.PoolManager.BulletPoolsList[indexBullet].DeSpawn(this);
            
        }
        if (collision.tag == GameConfig.BLOCK_OBJECT_TAG)
        {
            
                
                GameManageMent.Instance.PoolManager.BulletPoolsList[indexBullet].DeSpawn(this);
            
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

    
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg ;
        
        
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
