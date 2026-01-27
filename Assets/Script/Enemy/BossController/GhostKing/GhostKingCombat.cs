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
        if(dirType == DirType.RIGHT)
        {
            skill1Boss.transform.localPosition = new Vector3(rangeSpawnSkill1.x, 0f);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));

            skill1Boss.gameObject.GetComponent<SpriteRenderer>().flipX = true;

        }
        else if(dirType == DirType.LEFT)
        {
            skill1Boss.transform.localPosition = new Vector3(-rangeSpawnSkill1.x, 0f);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));

            skill1Boss.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if(dirType == DirType.DOWN)
        {
            skill1Boss.transform.localPosition = new Vector3(0, -rangeSpawnSkill1.y);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0,0,90));

            skill1Boss.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            skill1Boss.transform.localPosition = new Vector3(0, rangeSpawnSkill1.y);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0,0,90));

            skill1Boss.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        skill1Boss.gameObject.SetActive(true);

    }

    public void CastSkill2()
    {

        int turns = Random.Range(2, listTurnShootSkill2.Count + 1);
        StartCoroutine(CastSkill2AllTurn(7));

    }

    IEnumerator CastSkill2AllTurn(int turns)
    {
        for (int s = 0; s < turns; s++)
        {
            int turn =  Random.Range(0,listTurnShootSkill2.Count);
            TurnShoot curTurnShoot = listTurnShootSkill2[turn];
            float angleStep = 360 / curTurnShoot.amount;

            float currentAngle = curTurnShoot.first_Angle;

            for (int i = 0; i < curTurnShoot.amount; i++)
            {
                float dirX = Mathf.Cos(currentAngle * Mathf.Deg2Rad);

                float dirY = Mathf.Sin(currentAngle * Mathf.Deg2Rad);

                Vector2 dir = new Vector2(dirX, dirY).normalized;

                Vector2 posSpawn = (Vector2)this.transform.position + dir * rangeSpawnSkill2;
                for (int j = 0; j < curTurnShoot.amountBulletPerAngle; j++)
                {
                    BulletController bullet = GameManageMent.Instance.PoolManager.BulletPoolsList[idBulletSkill2].Spawn(posSpawn);


                    if (bullet != null)
                    {
                        bullet.SetInfo(curTurnShoot.damage, idBulletSkill2);
                        float speedVar = curTurnShoot.speedBullet + (j * 2f); 
                        bullet.SetSpeed(speedVar);

                        bullet.Fire(dir);
                        
                    }
                    else
                    {
                        Debug.LogError("CANNOT SPAWN BULLET FROM BOSS");
                    }
                }
                
                currentAngle += angleStep;
                if(curTurnShoot.delayAttack > 0f)
                {
                    yield return new WaitForSeconds(curTurnShoot.delayAttack );
                }

            }
            yield return new WaitForSeconds(delayPerTurnSkill2);
        }
    }


    public void CastSkill3()
    {
        
    }


    void Update()
    {
        // Test
        if (Input.GetKeyDown(KeyCode.J))
        {
            CastSkill1((GameManageMent.Instance.PlayerManager.PlayerController.getPos() - (Vector2)this.transform.position).normalized);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            CastSkill2();
        }
    }


}


