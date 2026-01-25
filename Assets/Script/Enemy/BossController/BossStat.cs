using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BossStat : MonoBehaviour
{
    [SerializeField] private EnemyBaseData bossData;

    public EnemyBaseData BossData => bossData;

    [SerializeField] private float currentHealth;
    public float CurrentHealth => currentHealth;

    private float targetHealth;

    [SerializeField] private bool isDead;

    public bool  IsDead => isDead;

    [SerializeField] private bool isAbsorbing;
    public bool IsAbsorbing => isAbsorbing;

    [SerializeField] private  Image healthUI;

    private void UpdateHealthUI()
    {
        healthUI.fillAmount = Mathf.Lerp( healthUI.fillAmount, targetHealth/bossData.MaxHealth, 0.1f);
    }



    public void TakeDamage(float damage,  bool isCrit)
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
            return;
        }
        targetHealth = currentHealth;

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
    void Update()
    {
        if(currentHealth/bossData.MaxHealth != healthUI.fillAmount)
        {
            UpdateHealthUI();
        }
    }


}
