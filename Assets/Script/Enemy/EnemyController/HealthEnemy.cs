using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HealthEnemy : MonoBehaviour
{
    
    [SerializeField] private float max_health;
    [SerializeField] private float cur_health;

    public float MaxHealth => max_health;
    public float CurHealth => cur_health;
    private Coroutine flashRoutine;
    private Material defaultMaterial;

    public void SetCurHealth(float health)
    {
        cur_health = health;
    }
    public void SetMaxHealth(float health)
    {
        max_health = health;
    }
    private void OnDamagedEffect()
    {
        GameManageMent.Instance.EffectController.Flash(this.GetComponent<SpriteRenderer>(), defaultMaterial, ref flashRoutine);

    }

    private void UpdateHealthUIEnemy()
    {
        float scale = cur_health / max_health;
        
        Transform childPos = this.transform.GetChild(1).transform;

        this.transform.GetChild(1).transform.DOScaleX(scale * 0.8f, 0.2f);
        this.transform.GetChild(1).gameObject.SetActive(true);
        this.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void OnDamaged(float damaged, bool isCritical = false)
    {

        if (cur_health < 0.1f)
        {
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
           
            int randomOffsetY = Random.Range(2, 6); 
            int randomOffsetX = Random.Range(-2,4);
            
            GameManageMent.Instance.PoolManager.FloatingTextPool.Spawn(Camera.main.WorldToScreenPoint(gameObject.transform.position+ new Vector3(randomOffsetX/2f, randomOffsetY/2, 0f) )).SetUp(((int)damaged).ToString(),Color.white, gradient, 16f);
            
        }
            
        else
        {
            Color normalTop = new Color(1f, 1f, 1f);       
           Color normalBot = new Color(0.8f, 0.8f, 0.8f); 
           var gradient = new TMPro.VertexGradient(normalTop, normalTop, normalTop, normalTop);
            
            int randomOffsetY = Random.Range(0, 6);
            int randomOffsetX = Random.Range(-2,4);
            GameManageMent.Instance.PoolManager.FloatingTextPool.Spawn(Camera.main.WorldToScreenPoint(gameObject.transform.position+ new Vector3(randomOffsetX/2f, randomOffsetY/2, 0f) )).SetUp(((int)damaged).ToString(), Color.white, gradient, 12f);
        }
        cur_health -= damaged;
        OnDamagedEffect();
        UpdateHealthUIEnemy();
        if (cur_health <= 0f)
        {
            cur_health = 0f;
            // Enemy die
            this.GetComponent<EnemyBase>().SetDie();
            
            GameManageMent.Instance.QuestManager.UpdateProgressAllQuestKill(1, this.GetComponent<EnemyBase>().EnemyBaseData.IndexEnemy, ObjectiveType.Kill);
            GameManageMent.Instance.PlayerManager.Gold.AddGold(this.GetComponent<EnemyBase>().EnemyBaseData.GoldValue);
            GameManageMent.Instance.PlayerManager.ExpSystem.GainExp(this.GetComponent<EnemyBase>().EnemyBaseData.ExpValue);
            //Nhan thuong
           
        }
    }
    void Awake()
    {
        defaultMaterial = this.GetComponent<SpriteRenderer>().material;
    }


}
