using System;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
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

    public bool IsDead => isDead;

    [SerializeField] private bool isAbsorbing;
    public bool IsAbsorbing => isAbsorbing;

    [SerializeField] private GameObject healthObject;

    [SerializeField] private Image healthUI;

    [SerializeField] private TextMeshProUGUI nameBossText;

    private Coroutine flashRoutine;
    private Material defaultMaterial;

    public event Action<float> OnHealthBossChange;

    public event Action OnBossDie;

    private void UpdateHealthUI()
    {
        healthUI.fillAmount = Mathf.Lerp(healthUI.fillAmount, targetHealth / bossData.MaxHealth, 0.1f);
    }

    private void OnDamagedEffect()
    {

        GameManageMent.Instance.EffectController.Flash(bossVisual.SpriteRenderer, defaultMaterial, ref flashRoutine);

    }



    public void TakeDamage(float damage, bool isCritical)
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
        Color color = Color.white;
        if (isCritical)
        {
            // Color critTop = new Color(1f, 0.4f, 0f);       
            // Color critBot = new Color(0.6f, 0f, 0f);
            var gradient = new TMPro.VertexGradient(
     new Color32(255, 235, 160, 255), // Top Left: Vàng kem sáng (Highlight)
     new Color32(255, 235, 160, 255), // Top Right
     new Color32(255, 140, 0, 255),   // Bottom Left: Cam đậm
     new Color32(255, 140, 0, 255)    // Bottom Right
 );

            int randomOffsetY = UnityEngine.Random.Range(2, 6);
            int randomOffsetX = UnityEngine.Random.Range(-2, 4);

            GameManageMent.Instance.PoolManager.FloatingTextPool.Spawn(Camera.main.WorldToScreenPoint(gameObject.transform.position + new Vector3(randomOffsetX / 2f, randomOffsetY / 2, 0f))).SetUp(((int)damage).ToString(), Color.white, gradient, 16f);

        }

        else
        {
            Color normalTop = new Color(1f, 1f, 1f);
            Color normalBot = new Color(0.8f, 0.8f, 0.8f);
            var gradient = new TMPro.VertexGradient(normalTop, normalTop, normalTop, normalTop);

            int randomOffsetY = UnityEngine.Random.Range(0, 6);
            int randomOffsetX = UnityEngine.Random.Range(-2, 4);
            GameManageMent.Instance.PoolManager.FloatingTextPool.Spawn(Camera.main.WorldToScreenPoint(gameObject.transform.position + new Vector3(randomOffsetX / 2f, randomOffsetY / 2, 0f))).SetUp(((int)damage).ToString(), Color.white, gradient, 12f);
        }

        currentHealth = Mathf.Max(0f, currentHealth - damage);


        targetHealth = currentHealth;
        
        if (currentHealth <= 0f)
        {
            Die();


        }
        OnHealthBossChange?.Invoke(currentHealth / bossData.MaxHealth);

    }
    public void Heal(float health)
    {
        if (isDead)
        {
            return;
        }
        currentHealth = Mathf.Min(bossData.MaxHealth, currentHealth + health);
        targetHealth = currentHealth;
        OnHealthBossChange?.Invoke(currentHealth / bossData.MaxHealth);
    }
    private void Die()
    {
        
        isDead = true;
        healthObject.SetActive(false);
        GameManageMent.Instance.QuestManager.UpdateProgressAllQuestKill(1, bossData.IndexEnemy, ObjectiveType.Kill);
        GameManageMent.Instance.PlayerManager.Gold.AddGold(bossData.GoldValue);
        GameManageMent.Instance.PlayerManager.ExpSystem.GainExp(bossData.ExpValue);
        GameManageMent.Instance._WorldManager.AddDefeatedBoss(bossData.IndexEnemy);
        OnBossDie?.Invoke();

    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    public void SetAbsorbingState(bool state)
    {
        isAbsorbing = state;
    }

    /// <summary>
    /// Reset boss stats to initial state
    /// </summary>
    public void ResetStats()
    {
        // Reset health
        currentHealth = bossData.MaxHealth;
        targetHealth = bossData.MaxHealth;

        // Reset states
        isDead = false;
        isAbsorbing = false;

        // Show health UI
        if (healthObject != null)
        {
            healthObject.SetActive(true);
        }

        // Reset health UI
        if (healthUI != null)
        {
            healthUI.fillAmount = 1f;
        }

        if (nameBossText != null)
        {
            nameBossText.text = bossData.NameEnemy;
        }
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
        
        if (currentHealth / bossData.MaxHealth != healthUI.fillAmount)
        {
            UpdateHealthUI();
        }
    }


}
