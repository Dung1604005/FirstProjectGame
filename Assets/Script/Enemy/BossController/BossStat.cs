using UnityEngine;
using UnityEngine.Rendering;

public class BossStat : MonoBehaviour
{
    [SerializeField] private EnemyBaseData bossData;

    public EnemyBaseData BossData => bossData;

    [SerializeField] private float currentHealth;
    public float CurrentHealth => currentHealth;

    [SerializeField] private bool isDead;

    public bool  IsDead => isDead;

    [SerializeField] private bool isAbsorbing;
    public bool IsAbsorbing => isAbsorbing;

   // [SerializeField] private 

    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }
        if (isAbsorbing)
        {
            Heal(damage);
        }
        currentHealth  = Mathf.Max(0f, currentHealth - damage);
        if(currentHealth <= 0f)
        {
            Die();
        }
    }
    public void Heal(float health)
    {
        if (isDead)
        {
            return;
        }
        currentHealth  = Mathf.Max(bossData.MaxHealth, currentHealth + health);
    }
    private void Die()
    {
        isDead = true;
    }
    public void  SetAbsorbingState(bool state)
    {
        isAbsorbing = state;
    }


}
