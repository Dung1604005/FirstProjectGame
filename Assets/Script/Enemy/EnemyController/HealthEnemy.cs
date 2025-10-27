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

    public void OnDamaged(float damaged)
    {

        if (cur_health < 0.1f)
        {
            return;
        }


        cur_health -= damaged;
        OnDamagedEffect();
        UpdateHealthUIEnemy();
        if (cur_health <= 0f)
        {
            cur_health = 0f;
            // Enemy die
            this.GetComponent<EnemyBase>().SetDie();
            this.gameObject.SetActive(false);
            
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
