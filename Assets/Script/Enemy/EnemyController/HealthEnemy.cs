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
            Debug.Log("Enemy Die" + this.GetComponent<EnemyBase>().EnemyBaseData.ExpValue);
            GameManageMent.Instance.PlayerManager.Gold.AddGold(this.GetComponent<EnemyBase>().EnemyBaseData.GoldValue);
            GameManageMent.Instance.PlayerManager.ExpSystem.GainExp(this.GetComponent<EnemyBase>().EnemyBaseData.ExpValue);
            //Nhan thuong
            var gradient = new TMPro.VertexGradient(
            new Color32(0xFF, 0xF8, 0xC5, 0xFF), // top-left
            new Color32(0xFF, 0xF8, 0xC5, 0xFF), // top-right
            new Color32(0xE5, 0xA1, 0x00, 0xFF), // bottom-left
            new Color32(0xFF, 0xD8, 0x4A, 0xFF)  // bottom-right
            );
            int randomOffsetY = Random.Range(0, 6);
            int randomOffsetX = Random.Range(-3,3);

        
            GameManageMent.Instance.PoolManager.FloatingTextPool.Spawn(Camera.main.WorldToScreenPoint((Vector3)GameManageMent.Instance.PlayerManager.PlayerController.getPos()+ new Vector3(randomOffsetX/2f, randomOffsetY/2, 0f) )).SetUp("+" + this.GetComponent<EnemyBase>().EnemyBaseData.GoldValue.ToString() + " GOLD", Color.yellow, gradient);



        }
    }
    void Awake()
    {
        defaultMaterial = this.GetComponent<SpriteRenderer>().material;
    }


}
