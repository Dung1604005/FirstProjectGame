using UnityEngine;

public class HuyBossManagement : MonoBehaviour
{
     [SerializeField] private BossStat bossStat;

    [SerializeField] private  BossVisual bossVisual;

    [SerializeField] private  BossMovement bossMovement;
    public BossMovement BossMovement => bossMovement;

    [SerializeField] private HuyCombat huyCombat;


    [SerializeField] private bool isAttacking= false;

    [Header("COOLDOWN")]

    [SerializeField] private float skill1CoolDownTimer;

    [SerializeField] private float skill2CoolDownTimer;

    [SerializeField] private float skill3CoolDownTimer;

    [SerializeField] private bool isActive;

    public bool IsActive => isActive;






     private void ChooseSkill1(){
        bossMovement.SetStationary(true);

        isAttacking = true;
    }

    public void EndSkill1()
    {
        bossMovement.SetStationary(false);
        
        skill1CoolDownTimer = 0f;
        isAttacking = false;
    }

     private void ChooseSkill2(){
        bossMovement.SetStationary(true);
        isAttacking = true;
    }

    public void EndSkill2()
    {
        bossMovement.SetStationary(false);
        skill2CoolDownTimer = 0f;
        isAttacking = false;
    }

     private void ChooseSkill3(){

        isAttacking = true;
    }

    public void EndSkill3()
    {
        skill3CoolDownTimer = 0f;
        isAttacking = false;
    }

    private void CastSkill()
    {
         if (isAttacking || bossMovement.Target == null)
        {
            return;
        }
        float rangeSqr = (bossMovement.Target.position - transform.position).sqrMagnitude;
        

        if(huyCombat && skill3CoolDownTimer >= huyCombat.Skill3CoolDown 
        && rangeSqr >= huyCombat.Skill3AttackRange*huyCombat.Skill3AttackRange)
        {
            ChooseSkill3();
            huyCombat.CastSkill3();
            return;
        }
        if(huyCombat && skill2CoolDownTimer >= huyCombat.Skill2CoolDown 
        && rangeSqr <= huyCombat.Skill2AttackRange*huyCombat.Skill2AttackRange)
        {
            ChooseSkill2();
            huyCombat.CastSkill2();
            return;
        }
       
        if(huyCombat && skill1CoolDownTimer >= huyCombat.Skill1CoolDown && rangeSqr <= huyCombat.Skill1AttackRange*huyCombat.Skill1AttackRange)
        {
            ChooseSkill1();
            huyCombat.CastSkill1(bossMovement.Target.position - transform.position);
            return;
        }
    }

    void Awake()
    {
        bossMovement = GetComponent<BossMovement>();
        bossStat = GetComponent<BossStat>();
        huyCombat = GetComponent<HuyCombat>();
    }
    void Start()
    {
        bossMovement.OnDashEnd+=EndSkill3;
        huyCombat.enabled = false;
        bossMovement.enabled = false;
        bossVisual.enabled = false;
        bossStat.enabled = false;
    }
    /// <summary>
    /// Reset Huy Boss to initial state
    /// </summary>
    public void ResetBoss()
    {
        // Reset attack state
        isAttacking = false;
        
        // Reset cooldown timers
        skill1CoolDownTimer = 0f;
        skill2CoolDownTimer = 0f;
        skill3CoolDownTimer = 0f;
        
        // Reset all components
        if (bossStat != null)
        {
            bossStat.ResetStats();
        }
        
        if (bossMovement != null)
        {
            bossMovement.ResetMovement();
        }
        
        if (bossVisual != null)
        {
            bossVisual.ResetVisual();
        }
    }

    public void ActiveBoss(){
        ResetBoss();
        huyCombat.enabled = true;
        bossMovement.enabled = true;
        bossVisual.enabled = true;
        bossStat.enabled = true;
        isActive = true;
    }

    public void DeActiveBoss(){
        huyCombat.enabled = false;
        bossMovement.enabled = false;
        bossVisual.enabled = false;
        bossStat.enabled = false;
        isActive = false;

    }

    void Update()
    {
         if(GameManageMent.Instance.GameState == GameState.Pause || !isActive)
        {
            return;
        }
        if(skill1CoolDownTimer < huyCombat.Skill1CoolDown)
        {
            skill1CoolDownTimer += Time.deltaTime;
        }
        if(skill2CoolDownTimer <  huyCombat.Skill2CoolDown)
        {
            skill2CoolDownTimer += Time.deltaTime;
        }
        if(skill3CoolDownTimer <  huyCombat.Skill3CoolDown)
        {
            skill3CoolDownTimer += Time.deltaTime;
        }
         
        CastSkill();
    }
}
