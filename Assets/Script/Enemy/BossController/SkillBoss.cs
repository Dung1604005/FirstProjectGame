using UnityEngine;

public class SkillBoss : MonoBehaviour, IPoolable
{
    [SerializeField] private float damage;

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        
        if (collider2D.tag == GameConfig.HITBOX_PLAYER){
            
                    
            if (collider2D.gameObject != null){
                collider2D.gameObject.GetComponentInParent<Health>()?.OnDamaged(damage);
            }                    
        }
    }

    public void EndAttack()
    {
        this.gameObject.SetActive(false);
    }

    public void OnSpawn()
    {
        
    }
    public void OnDeSpawn()
    {
        

    }
}
