using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;

public class GhostKingCombat : MonoBehaviour
{
    [Header("SKILL 1")]

    [SerializeField] private float skill1Damage;

    [SerializeField] private float skill1AttackRange;

    [SerializeField] private float skill1CoolDown;

    [SerializeField] private float delayAttack;

    [SerializeField] private Vector2 rangeSpawnSkill1;

    [SerializeField] private SkillBoss skill1Boss;


    [Header("SKILL 2")]

    [SerializeField] private float skill2AttackRange;

    [SerializeField] private float skill2CoolDown;

    [SerializeField] private float rangeSpawnSkill2;

    [SerializeField] private List<TurnShoot> listTurnShootSkill2;

    [SerializeField] private int idBulletSkill2;

    [SerializeField] private float delayPerTurnSkill2;

    [Header("SKILL 3")]

    [SerializeField] private float skill3AttackRange;

    [SerializeField] private float skill3CoolDown;

    [SerializeField] private float rangeSpawnSkill3;

    [SerializeField] private List<TurnShoot> listTurnSpawnSkill3;

    [SerializeField] private float delayPerTurnSkill3;





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

    }

    public void CastSkill2()
    {

        int turns = Random.Range(4, 10);
        StartCoroutine(CastSkill2AllTurn(turns));

    }

    IEnumerator CastSkill2AllTurn(int turns)
    {
        for (int s = 0; s < turns; s++)
        {
            int turn = Random.Range(0, listTurnShootSkill2.Count);
            TurnShoot curTurnShoot = listTurnShootSkill2[turn];
            float angleStep = 360 / curTurnShoot.amount;

            float currentAngle = curTurnShoot.first_Angle;

            for (int i = 0; i < curTurnShoot.amount; i++)
            {
                float dirX = Mathf.Cos(currentAngle * Mathf.Deg2Rad);

                float dirY = Mathf.Sin(currentAngle * Mathf.Deg2Rad);

                Vector2 dir = new Vector2(dirX, dirY).normalized;

                StartCoroutine(CastSkill2OneAngle(curTurnShoot, dir, dir * rangeSpawnSkill2, (Vector2)this.transform.position));

                currentAngle += angleStep;
                if (curTurnShoot.delayAttack > 0f)
                {
                    yield return new WaitForSeconds(curTurnShoot.delayAttack);
                }

            }
            yield return new WaitForSeconds(delayPerTurnSkill2);
        }
    }

    IEnumerator CastSkill2OneAngle(TurnShoot curTurnShoot,Vector2 dir,  Vector2 jumpValue, Vector2 pos)
    {
        for (int j = 1; j <= curTurnShoot.amountBulletPerAngle; j++)
        {
            Vector2 posSpawn = pos + jumpValue*j;
            BulletController bullet = GameManageMent.Instance.PoolManager.BulletPoolsList[idBulletSkill2].Spawn(posSpawn);
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
        (skillBoss as GroundZoneController).SetActive(true);
        for (int i = 0; i < curTurnShoot.amount; i++)
        {
            float dirX = Mathf.Cos(currentAngle * Mathf.Deg2Rad);

            float dirY = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            Vector2 dir = new Vector2(dirX, dirY).normalized;
            StartCoroutine(CastSkill3OneAngle(curTurnShoot.amountBulletPerAngle, curTurnShoot.delayAttackInOneAngle, dir * rangeSpawnSkill3, posSpawn));

            currentAngle += angleStep;
            if (curTurnShoot.delayAttack > 0f)
            {
                yield return new WaitForSeconds(curTurnShoot.delayAttack);
            }

        }
    }
    IEnumerator CastSkill3OneAngle(int amount, float delay, Vector2 jumpValue, Vector2 pos)
    {
        for (int j = 1; j <= amount; j++)
        {

            Vector2 posSpawn = pos + jumpValue * j;
            SkillBoss skillBoss = GameManageMent.Instance.PoolManager.Skill3GhostKingPool.Spawn(posSpawn);
            (skillBoss as GroundZoneController).SetActive(true);
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(delay);
        }
    }



    void Update()
    {
        // Test
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (GameObject.FindGameObjectWithTag(GameConfig.PLAYER_TAG0) != null)
            {


            }
            CastSkill1((GameManageMent.Instance.PlayerManager.PlayerController.getPos() - (Vector2)this.transform.position).normalized);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            CastSkill2();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (GameObject.FindGameObjectWithTag(GameConfig.PLAYER_TAG0) != null)
            {
                CastSkill3(GameManageMent.Instance.PlayerManager.PlayerController.getPos());
            }

        }
    }


}


