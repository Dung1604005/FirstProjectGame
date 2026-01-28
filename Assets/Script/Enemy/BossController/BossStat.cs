using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BossStat : MonoBehaviour
{
    [SerializeField] private EnemyBaseData bossData;

    public EnemyBaseData BossData => bossData;

    [SerializeField] private BossVisual bossVisual;

    [SerializeField] private float currentHealth;
    public float CurrentHealth => currentHealth;

    private float targetHealth;

    [SerializeField] private bool isDead;

    public bool  IsDead => isDead;

    [SerializeField] private bool isAbsorbing;
    public bool IsAbsorbing => isAbsorbing;

    [SerializeField] private  Image healthUI;

    private Coroutine flashRoutine;
    private Material defaultMaterial;

    private void UpdateHealthUI()
    {
        healthUI.fillAmount = Mathf.Lerp( healthUI.fillAmount, targetHealth/bossData.MaxHealth, 0.1f);
    }

    private void OnDamagedEffect()
    {
        
        GameManageMent.Instance.EffectController.Flash(bossVisual.SpriteRenderer, defaultMaterial, ref flashRoutine);

    }



    public void TakeDamage(float damage,  bool isCrit)
    {
        if (isDead)
        {
            return;
        }
        OnDamagedEffect();
        if (isAbsorbing)
        {
            Heal(damage);
            return;
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
        currentHealth  = Mathf.Min(bossData.MaxHealth, currentHealth + health);
    }
    private void Die()
    {
        
        isDead = true;
        bossVisual.SetDie();
        
    
    }
    public void  SetAbsorbingState(bool state)
    {
        isAbsorbing = state;
    }
    void Awake()
    {
        currentHealth = bossData.MaxHealth;
        targetHealth = bossData.MaxHealth;
    }
    void Start()
    {
        defaultMaterial = bossVisual.SpriteRenderer
        .material;
    }
    void Update()
    {
        if(currentHealth/bossData.MaxHealth != healthUI.fillAmount)
        {
            UpdateHealthUI();
        }
    }


}
