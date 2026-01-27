using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class Health : MonoBehaviour
{
    [SerializeField] private float max_health;
    [SerializeField] private float cur_health;

    // Su kien thay doi HP
    public UnityEvent<float, float> OnHealthChanged = new UnityEvent<float, float>();

    private SpriteRenderer spriteRenderer;
    private Color defaultColor;
     private Material defaultMaterial;


    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration;
    private Coroutine flashRoutine;
    public void SetCurHp(float hp)
    {
        cur_health = hp;
    }
    public float CurHp()
    {
        return cur_health;
    }
    public float MaxHp()
    {
        return max_health;
    }
    
    
    private void OnDamagedEffect()
    {
        GameManageMent.Instance.EffectController.Flash(this.GetComponent<SpriteRenderer>(), defaultMaterial, ref flashRoutine);

    }

    private void OnHealEffect()
    {
        spriteRenderer.DOKill();

        spriteRenderer.DOColor(Color.green, 0.05f).SetLoops(2, LoopType.Yoyo).OnComplete(() => spriteRenderer.color = defaultColor);
    }

    
    public void OnDamaged(float damaged)
    {
        
        if (cur_health < 0.1f)
        {
            return;
        }
       
        cur_health -= damaged;
        
        if (cur_health < 0f)
        {
            cur_health = 0f;


        }
        OnDamagedEffect();
        if (this.gameObject.tag == GameConfig.PLAYER_TAG0)
        {
            OnHealthChanged.Invoke(cur_health, max_health);
        }

        // Build them truong hop pha huy vat can
        //Kiem tra va cham de dieu chinh thanh mau

        

        // Hoat anh chet
        if (cur_health <= 0.1f)
        {
            if (this.tag == GameConfig.DESTROYABLE_OBJECT_TAG)
            {
                Destroy(gameObject);
            }
        }


    }
    //Hoi mau
    public void OnHeal(float heal)
    {
        if (cur_health + heal > max_health)
        {
            cur_health = max_health;
        }
        else
        {
            cur_health = cur_health + heal;
        }
        OnHealEffect();
        if (this.gameObject.tag == GameConfig.PLAYER_TAG0)
        {
            OnHealthChanged.Invoke(cur_health, max_health);
        }

    }
    // Tang mau toi da
    public void SetMaxHp(float hp, bool start = false)
    {
        max_health = hp;
        if (start)
        {
            cur_health = max_health;
        }
        
        if (this.gameObject.tag == GameConfig.PLAYER_TAG0)
        {
            
            OnHealthChanged.Invoke(cur_health, max_health);
        }

    }

    public void OnDied()
    {
        Destroy(this.gameObject);

    }
    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        defaultMaterial = spriteRenderer.material;

    }
    void Start()
    {
        cur_health = max_health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
