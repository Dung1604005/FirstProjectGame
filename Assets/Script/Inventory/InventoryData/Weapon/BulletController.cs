using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{


    [SerializeField] private float moveSpeed= 5f;
    [SerializeField] private float exist_time = 2f;

    [SerializeField] private float damaged;

    
    
    private Rigidbody2D rb;

    public void SetDamaged(float _damaged)
    {
        this.damaged=_damaged;
    }
    //Kiem tra va cham voi dich
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Damaged");
        if (collision.tag == GameConfig.HITBOX_ENEMY)
        {
            
            collision.gameObject.GetComponentInParent<Health>().OnDamaged(damaged);
        }
        if (collision.tag == GameConfig.DESTROYABLE_OBJECT_TAG)
        {
            collision.gameObject.GetComponent<Health>().OnDamaged(damaged);
        }

        Destroy(this.gameObject);
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(exist_time);
        Destroy(this.gameObject);
    }
    
    // Ban theo huong
    public void Fire(Vector2 dir)
    {

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg ;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.velocity = dir * moveSpeed;

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
