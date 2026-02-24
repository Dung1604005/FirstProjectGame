using System.Collections;

using NUnit.Framework.Internal;
using UnityEngine;

public class HuyCombat : MonoBehaviour
{

    private HuyBossManagement huyBossManagement;
    [Header("SKILL 1")]

    [SerializeField] private float skill1Damage;

    [SerializeField] private float skill1AttackRange;

    public float Skill1AttackRange => skill1AttackRange;

    [SerializeField] private float skill1CoolDown;

    public float Skill1CoolDown => skill1CoolDown;

    [SerializeField] private float delayAttackSkill1;

    [SerializeField] private Vector2 rangeSpawnSkill1;

    [SerializeField] private SkillBoss skill1Boss;

    public SkillBoss Skill1Boss => skill1Boss;



    [Header("SKILL 2")]

    [SerializeField] private float skill2Damage;

    [SerializeField] private float skill2AttackRange;

    public float Skill2AttackRange => skill2AttackRange;

    [SerializeField] private float skill2CoolDown;

    public float Skill2CoolDown => skill2CoolDown;

    [SerializeField] private float delayAttackSkill2;

    [SerializeField] private Vector2 rangeSpawnSkill2;

    [SerializeField] private int idBulletSkill2;

    [Header("SKILL 3")]

    [SerializeField] private float skill3CoolDown;

    public float Skill3CoolDown => skill3CoolDown;

    [SerializeField] private float delayAttackSkill3;

    [SerializeField] private float skill3MinAttackRange;

    public float Skill3MinAttackRange => skill3MinAttackRange;

    [SerializeField] private float skill3MaxAttackRange;

    public float Skill3MaxAttackRange => skill3MaxAttackRange;


    [SerializeField] private GameObject warningIcon;
    public GameObject WarningIcon => warningIcon;



    void Awake()
    {
        huyBossManagement = GetComponent<HuyBossManagement>();
        skill1Boss.gameObject.SetActive(false);
        warningIcon.SetActive(false);
    }

    void Init()
    {
        skill1Boss.SetDamage(skill1Damage);

    }

    void Start()
    {
        Init();
    }


    public void CastSkill1(Vector2 dir)
    {
        StartCoroutine(CastSkill1Coroutine(dir));

    }
    IEnumerator CastSkill1Coroutine(Vector2 dir)
    {
        warningIcon.SetActive(true);
        yield return new WaitForSeconds(delayAttackSkill1);

        warningIcon.SetActive(false);
        DirType dirType = GameManageMent.Instance.CalculateDirType(dir.x, dir.y);
        if (dirType == DirType.RIGHT)
        {
            skill1Boss.transform.localPosition = new Vector3(rangeSpawnSkill1.x, 0f);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            skill1Boss.gameObject.transform.localScale = new Vector3(-1, 1, 0);

        }
        else if (dirType == DirType.LEFT)
        {
            skill1Boss.transform.localPosition = new Vector3(-rangeSpawnSkill1.x, 0f);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            skill1Boss.gameObject.transform.localScale = new Vector3(1, 1, 0);
        }
        else if (dirType == DirType.DOWN)
        {
            skill1Boss.transform.localPosition = new Vector3(0, -rangeSpawnSkill1.y);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

            skill1Boss.gameObject.transform.localScale = new Vector3(1, 1, 0);
        }
        else
        {
            skill1Boss.transform.localPosition = new Vector3(0, rangeSpawnSkill1.y);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            skill1Boss.gameObject.transform.localScale = new Vector3(-1, 1, 0);


        }

        
        skill1Boss.gameObject.SetActive(true);
        huyBossManagement.EndSkill1();

    }

    public void CastSkill2()
    {
        StartCoroutine(CastSkill2Coroutine());
    }




    IEnumerator CastSkill2Coroutine()
    {
        if(huyBossManagement.BossMovement.Target == null)
        {
            huyBossManagement.EndSkill2();
            yield break;
        }
        
        warningIcon.SetActive(true);
        yield return new WaitForSeconds(delayAttackSkill2);
        warningIcon.SetActive(false);
        Vector2 dir = (GameManageMent.Instance.PlayerManager.PlayerController.getPos() - (Vector2)this.transform.position).normalized;

        Vector2 pos = (Vector2)this.transform.position + dir;
        BulletController bullet = GameManageMent.Instance.PoolManager.BulletPoolsList[idBulletSkill2].Spawn(pos);
        if (bullet != null)
        {
            bullet.SetInfo(skill2Damage, idBulletSkill2);
            bullet.Fire(dir);
        }
        else
        {
            Debug.LogError("CANNOT SPAWN BULLET FROM BOSS");
        }
        huyBossManagement.EndSkill2();
    }

    public void CastSkill3()
    {
        StartCoroutine(CastSkill3Routine());
    }

    IEnumerator CastSkill3Routine()
    {
        Vector2 posTarget = Vector2.zero;
        if(huyBossManagement.BossMovement.Target != null)
        {
            posTarget = huyBossManagement.BossMovement.Target.position - this.transform.position;
        }
        else
        {
            huyBossManagement.EndSkill3();
            yield break;
        }
        warningIcon.SetActive(true);

        yield return new WaitForSeconds(delayAttackSkill3);
        warningIcon.SetActive(false);
        Debug.Log("Dash To " + posTarget);

        huyBossManagement.BossMovement.StartDash(posTarget);

        
        




    }


    void Update()
    {
        // Test
        
    }
}
