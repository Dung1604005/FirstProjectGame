using UnityEngine;

public class HuyBossManagement : MonoBehaviour, BossManagerInterface
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



    public void Restore(int id)
    {
        if(id != bossStat.BossData.IndexEnemy)
        {
            return;
        }
        ActiveBoss();
        bossStat.TakeDamage(99999, false);
    }

     private void ChooseSkill1(){
        bossMovement.SetStationary(true);
        skill1CoolDownTimer = 0f;

        isAttacking = true;
    }

    public void EndSkill1()
    {
        bossMovement.SetStationary(false);
        
        skill1CoolDownTimer = 0f;
        isAttacking = false;
    }

     private void ChooseSkill2(){
        skill2CoolDownTimer = 0f;
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
        skill3CoolDownTimer = 0f;

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
        

        if(skill3CoolDownTimer >= huyCombat.Skill3CoolDown 
        && rangeSqr >= huyCombat.Skill3MinAttackRange*huyCombat.Skill3MinAttackRange && 
        rangeSqr <= huyCombat.Skill3MaxAttackRange*huyCombat.Skill3MaxAttackRange)
        {
            ChooseSkill3();
            huyCombat.CastSkill3();
            return;
        }
        if( skill2CoolDownTimer >= huyCombat.Skill2CoolDown 
        && rangeSqr <= huyCombat.Skill2AttackRange*huyCombat.Skill2AttackRange)
        {
            ChooseSkill2();
            huyCombat.CastSkill2();
            return;
        }
       
        if(skill1CoolDownTimer >= huyCombat.Skill1CoolDown && rangeSqr <= huyCombat.Skill1AttackRange*huyCombat.Skill1AttackRange)
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
        bossStat.OnBossDie += BossDie;
        huyCombat.enabled = false;
        bossMovement.enabled = false;
        
        bossStat.enabled = false;

        GameManageMent.Instance._WorldManager.OnLoadDataBoss += Restore;
    }
    /// <summary>
    /// Reset Huy Boss to initial state
    /// </summary>
    public void ResetBoss()
    {
        // Reset attack state
        isAttacking = false;
        
        // Reset cooldown timers
        skill1CoolDownTimer = huyCombat.Skill1CoolDown/2f;
        skill2CoolDownTimer = huyCombat.Skill2CoolDown/2f;
        skill3CoolDownTimer = huyCombat.Skill3CoolDown/2f;
        
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
        if (bossStat.IsDead)
        {
            return;
        }
        ResetBoss();
       
        huyCombat.Skill1Boss.gameObject.SetActive(false);
        huyCombat.WarningIcon.gameObject.SetActive(false);
        
        huyCombat.enabled = true;
        
        bossMovement.enabled = true;
        
        bossStat.enabled = true;
        isActive = true;
        isAttacking = false;
    }

    public void DeActiveBoss(){
        huyCombat.StopAllCoroutines();
        bossStat.StopAllCoroutines();
        bossMovement.StopAllCoroutines();
        huyCombat.Skill1Boss.gameObject.SetActive(false);
        huyCombat.WarningIcon.gameObject.SetActive(false);
        huyCombat.enabled = false;
        bossMovement.enabled = false;
        
        bossStat.enabled = false;
        isActive = false;

    }

    public void BossDie()
    {
        this.GetComponent<SenderEvent>().SendEvent();
        DeActiveBoss();
        this.GetComponent<SenderEvent>().RecallEvent();
    }

    void Update()
    {
         if(GameManageMent.Instance.GameState == GameState.Pause || !isActive || bossStat.IsDead)
        {
            return;
        }
        if(skill1CoolDownTimer <= huyCombat.Skill1CoolDown)
        {
            skill1CoolDownTimer += Time.deltaTime;
        }
        if(skill2CoolDownTimer <=  huyCombat.Skill2CoolDown)
        {
            skill2CoolDownTimer += Time.deltaTime;
        }
        if(skill3CoolDownTimer <=  huyCombat.Skill3CoolDown)
        {
            skill3CoolDownTimer += Time.deltaTime;
        }
         
        CastSkill();
    }
}
