using UnityEngine;

public class GhostKingManager : MonoBehaviour
{

    [SerializeField] private BossStat bossStat;

    [SerializeField] private  BossVisual bossVisual;

    [SerializeField] private  BossMovement bossMovement;

    [SerializeField] private GhostKingCombat ghostKingCombat;


    [SerializeField] private bool isAttacking= false;

    [Header("COOLDOWN")]

    [SerializeField] private float skill1CoolDownTimer;

    [SerializeField] private float skill2CoolDownTimer;

    [SerializeField] private float skill3CoolDownTimer;

    [SerializeField] private float skill4CoolDownTimer;



    public void InitOutSide()
    {
        
        GameManageMent.Instance.PoolManager.InitSkillGhostKing();
        skill1CoolDownTimer = ghostKingCombat.Skill1CoolDown;
        skill2CoolDownTimer = ghostKingCombat.Skill2CoolDown;
        skill3CoolDownTimer = ghostKingCombat.Skill3CoolDown;
        skill4CoolDownTimer = ghostKingCombat.Skill4CoolDown;
        bossStat.OnHealthBossChange += UnlockedSkill;


    }

    private void ChooseSkill1(){

        isAttacking = true;
    }

    public void EndSkill1()
    {
        skill1CoolDownTimer = 0f;
        isAttacking = false;
    }

    private void ChooseSkill2()
    {
        bossMovement.SetStationary(true);
        bossVisual.StartShakingNoise();
        isAttacking = true;
        bossVisual.SetAnimAttack(true);
    }

    public void StartTurnSkill2()
    {
        bossVisual.PlayChargeEffect(true);
    }

    public void EndChargeTurnSkill2()
    {
        bossVisual.PlayChargeEffect(false);
        bossVisual.PlayShootEffect();
    }

    public void EndSkill2()
    {
        skill2CoolDownTimer = 0f;
        isAttacking = false;
        bossMovement.SetStationary(false);
        bossVisual.PlayChargeEffect(false);
        bossVisual.SetAnimAttack(false);
    }

    private void ChooseSkill3()
    {
        isAttacking = true;
        bossMovement.SetStationary(true);
        bossVisual.SetAnimAttack(true);
    }

    public void EndSkill3()
    {
        skill3CoolDownTimer = 0f;
        isAttacking = false;
        bossMovement.SetStationary(false);
        bossVisual.SetAnimAttack(false);
    }

    private void ChooseSkill4()
    {
        bossStat.SetAbsorbingState(true);
        skill4CoolDownTimer = 0f;
        
    }

    public void EndSkill4()
    {
        skill4CoolDownTimer = 0f;

        bossStat.SetAbsorbingState(false);
        
    }

    public void UnlockedSkill(float healthPercentage)
    {
        if(healthPercentage <= 0.75f)
        {
            ghostKingCombat.UnlockSkill3();
        }
        if(healthPercentage  <= 0.5f)
        {
            ghostKingCombat.UnlockSkill4();
        }
    }
    

    private void CastSkill()
    {
        if (isAttacking || bossMovement.Target == null)
        {
            return;
        }
        float rangeSqr = (bossMovement.Target.position - transform.position).sqrMagnitude;
        if(ghostKingCombat.UnlockedSkill4 && skill4CoolDownTimer >= ghostKingCombat.Skill4CoolDown 
        && rangeSqr <= ghostKingCombat.Skill4AttackRange*ghostKingCombat.Skill4AttackRange)
        {
            ChooseSkill4();
            ghostKingCombat.CastSkill4();
            return;
        }

        if(ghostKingCombat.UnlockedSkill3 && skill3CoolDownTimer >= ghostKingCombat.Skill3CoolDown 
        && rangeSqr <= ghostKingCombat.Skill3AttackRange*ghostKingCombat.Skill3AttackRange)
        {
            ChooseSkill3();
            ghostKingCombat.CastSkill3(bossMovement.Target.position);
            return;
        }
        if(ghostKingCombat.UnlockedSkill2 && skill2CoolDownTimer >= ghostKingCombat.Skill2CoolDown 
        && rangeSqr <= ghostKingCombat.Skill2AttackRange*ghostKingCombat.Skill2AttackRange)
        {
            ChooseSkill2();
            ghostKingCombat.CastSkill2();
            return;
        }
       
        if(ghostKingCombat.UnlockedSkill1 && skill1CoolDownTimer >= ghostKingCombat.Skill1CoolDown && rangeSqr <= ghostKingCombat.Skill1AttackRange*ghostKingCombat.Skill1AttackRange)
        {
            ChooseSkill1();
            ghostKingCombat.CastSkill1(bossMovement.Target.position - transform.position);
            return;
        }

    }

    void Start()
    {
        InitOutSide();
    }

    void Update()
    {
        if(GameManageMent.Instance.GameState == GameState.Pause)
        {
            return;
        }
        if(skill1CoolDownTimer < ghostKingCombat.Skill1CoolDown)
        {
            skill1CoolDownTimer += Time.deltaTime;
        }
        if(skill2CoolDownTimer < ghostKingCombat.Skill2CoolDown)
        {
            skill2CoolDownTimer += Time.deltaTime;
        }
        if(skill3CoolDownTimer < ghostKingCombat.Skill3CoolDown)
        {
            skill3CoolDownTimer += Time.deltaTime;
        }
        if(skill4CoolDownTimer < ghostKingCombat.Skill4CoolDown)
        {
            skill4CoolDownTimer += Time.deltaTime;
        }
        CastSkill();
    }
}
