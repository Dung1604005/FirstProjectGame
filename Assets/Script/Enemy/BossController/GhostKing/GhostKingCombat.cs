using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GhostKingCombat : MonoBehaviour
{

    [SerializeField] private GhostKingManager ghostKingManager;
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
    
    [SerializeField] private bool unlockedSkill1;

    public bool UnlockedSkill1 => unlockedSkill1;


    [Header("SKILL 2")]

    [SerializeField] private float skill2AttackRange;

    public float Skill2AttackRange => skill2AttackRange;

    [SerializeField] private float skill2CoolDown;

    public float Skill2CoolDown => skill2CoolDown;

    [SerializeField] private float rangeSpawnSkill2;

    [SerializeField] private List<TurnShoot> listTurnShootSkill2;

    [SerializeField] private int idBulletSkill2;

    [SerializeField] private float delayPerTurnSkill2;

    [SerializeField] private bool unlockedSkill2;

    public bool UnlockedSkill2 => unlockedSkill2;

    [Header("SKILL 3")]

    [SerializeField] private float skill3AttackRange;

    public float Skill3AttackRange => skill3AttackRange;

    [SerializeField] private float skill3CoolDown;

    public float Skill3CoolDown => skill3CoolDown;

    [SerializeField] private float rangeSpawnSkill3;

    [SerializeField] private List<TurnShoot> listTurnSpawnSkill3;

    [SerializeField] private float delayPerTurnSkill3;

    [SerializeField] private bool unlockedSkill3;

    public bool UnlockedSkill3 => unlockedSkill3;

    [Header("SKILL 4")]

    [SerializeField] private float skill4AttackRange;

    public float Skill4AttackRange => skill4AttackRange;

    [SerializeField] private float skill4CoolDown;

    public float Skill4CoolDown => skill4CoolDown;

    [SerializeField] private float skill4Duration;

    [SerializeField] private GameObject skill4Prefab;

    [SerializeField] private bool unlockedSkill4;

    public bool UnlockedSkill4 => unlockedSkill4;


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
        
        yield return new WaitForSeconds(delayAttackSkill1);
        DirType dirType = GameManageMent.Instance.CalculateDirType(dir.x, dir.y);
        if (dirType == DirType.RIGHT)
        {
            skill1Boss.transform.localPosition = new Vector3(rangeSpawnSkill1.x, 0f);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            skill1Boss.gameObject.GetComponent<SpriteRenderer>().flipX = true;

        }
        else if (dirType == DirType.LEFT)
        {
            skill1Boss.transform.localPosition = new Vector3(-rangeSpawnSkill1.x, 0f);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            skill1Boss.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (dirType == DirType.DOWN)
        {
            skill1Boss.transform.localPosition = new Vector3(0, -rangeSpawnSkill1.y);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

            skill1Boss.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            skill1Boss.transform.localPosition = new Vector3(0, rangeSpawnSkill1.y);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

            skill1Boss.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        skill1Boss.gameObject.SetActive(true);
        ghostKingManager.EndSkill1();
    }

    public void CastSkill2()
    {

        int turns = Random.Range(3, 5);
        StartCoroutine(CastSkill2AllTurn(turns));

    }

    IEnumerator CastSkill2AllTurn(int turns)
    {
        
        ghostKingManager.StartTurnSkill2();
        yield return new WaitForSeconds(delayPerTurnSkill2);
        
        for (int s = 0; s < turns; s++)
        {
            ghostKingManager.EndChargeTurnSkill2();
            int turn = Random.Range(0, listTurnShootSkill2.Count);
            TurnShoot curTurnShoot = listTurnShootSkill2[turn];
            float angleStep = 360 / curTurnShoot.amount;

            float currentAngle = curTurnShoot.first_Angle;

            for (int i = 0; i < curTurnShoot.amount; i++)
            {
                float dirX = Mathf.Cos(currentAngle * Mathf.Deg2Rad);

                float dirY = Mathf.Sin(currentAngle * Mathf.Deg2Rad);

                Vector2 dir = new Vector2(dirX, dirY).normalized;

                StartCoroutine(CastSkill2OneAngle(curTurnShoot, dir, (Vector2)this.transform.position));

                currentAngle += angleStep;
                if (curTurnShoot.delayAttack > 0f)
                {
                    yield return new WaitForSeconds(curTurnShoot.delayAttack);
                }

            }
            if(s < turns - 1)
            {
                 ghostKingManager.StartTurnSkill2();
            }
            

            yield return new WaitForSeconds(delayPerTurnSkill2);
        }

        ghostKingManager.EndSkill2();
    }

    IEnumerator CastSkill2OneAngle(TurnShoot curTurnShoot,Vector2 dir, Vector2 pos)
    {
        for (int j = 1; j <= curTurnShoot.amountBulletPerAngle; j++)
        {
            
            BulletController bullet = GameManageMent.Instance.PoolManager.BulletPoolsList[idBulletSkill2].Spawn(pos);
            if (bullet != null)
            {
                bullet.SetInfo(curTurnShoot.damage, idBulletSkill2);
                float speedVar = curTurnShoot.speedBullet;
                bullet.SetSpeed(speedVar);
                bullet.Fire(dir);
            }
            else
            {
                Debug.LogError("CANNOT SPAWN BULLET FROM BOSS");
            }
            if (curTurnShoot.delayAttackInOneAngle > 0f)
            {
                yield return new WaitForSeconds(curTurnShoot.delayAttackInOneAngle);
            }
            yield return new WaitForSeconds(curTurnShoot.delayAttackInOneAngle);
        }
    }

    public void UnlockSkill3()
    {
        unlockedSkill3 = true;
    }


    public void CastSkill3(Vector2 posSpawn)
    {
        int turn = Random.Range(0, listTurnSpawnSkill3.Count);
        StartCoroutine(CastSkill3Coroutine(turn, posSpawn));
    }

    IEnumerator CastSkill3Coroutine(int turn, Vector2 posSpawn)
    {
        TurnShoot curTurnShoot = listTurnSpawnSkill3[turn];
        float angleStep = 360 / curTurnShoot.amount;
        float currentAngle = curTurnShoot.first_Angle;

        SkillBoss skillBoss = GameManageMent.Instance.PoolManager.Skill3GhostKingPool.Spawn(posSpawn);
        skillBoss.SetDamage(curTurnShoot.damage);
        
        (skillBoss as GroundZoneController).SetActive(true);
        for (int i = 0; i < curTurnShoot.amount; i++)
        {
            float dirX = Mathf.Cos(currentAngle * Mathf.Deg2Rad);

            float dirY = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            Vector2 dir = new Vector2(dirX, dirY).normalized;
            StartCoroutine(CastSkill3OneAngle(curTurnShoot.amountBulletPerAngle,curTurnShoot.damage, curTurnShoot.delayAttackInOneAngle, dir * rangeSpawnSkill3, posSpawn));

            currentAngle += angleStep;
            if (curTurnShoot.delayAttack > 0f)
            {
                yield return new WaitForSeconds(curTurnShoot.delayAttack);
            }
        }
        ghostKingManager.EndSkill3();
    }
    IEnumerator CastSkill3OneAngle(int amount,float damage, float delay, Vector2 jumpValue, Vector2 pos)
    {
        for (int j = 1; j <= amount; j++)
        {

            Vector2 posSpawn = pos + jumpValue * j;
            SkillBoss skillBoss = GameManageMent.Instance.PoolManager.Skill3GhostKingPool.Spawn(posSpawn);
            skillBoss.SetDamage(damage);
            (skillBoss as GroundZoneController).SetActive(true);
            
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(delay);
        }
    }

    public void UnlockSkill4()
    {
        unlockedSkill4 = true;
    }

    public void CastSkill4()
    {
        skill4Prefab.SetActive(true);
        StartCoroutine(CastSkill4Coroutine());
    }
    IEnumerator CastSkill4Coroutine()
    {
        yield return new WaitForSeconds(skill4Duration);
        skill4Prefab.SetActive(false);
        ghostKingManager.EndSkill4();
    }
    public void ResetGhostKingCombat()
    {
        unlockedSkill3 = false;
        unlockedSkill4 = false;
    }




    void Update()
    {
        // Test
        // if (Input.GetKeyDown(KeyCode.J))
        // {
        //     if (GameObject.FindGameObjectWithTag(GameConfig.PLAYER_TAG0) != null)
        //     {


        //     }
        //     CastSkill1((GameManageMent.Instance.PlayerManager.PlayerController.getPos() - (Vector2)this.transform.position).normalized);
        // }

        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     CastSkill2();
        // }
        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     if (GameObject.FindGameObjectWithTag(GameConfig.PLAYER_TAG0) != null)
        //     {
        //         CastSkill3(GameManageMent.Instance.PlayerManager.PlayerController.getPos());
        //     }

        // }
    }


}


