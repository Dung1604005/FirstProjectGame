using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.MPE;
using UnityEngine;

public class GhostKingCombat : MonoBehaviour
{
    [Header("SKILL 1")]

    [SerializeField] private float skill1Damage;

    [SerializeField] private float skill1AttackRange;

    [SerializeField] private float skill1CoolDown;

    [SerializeField] private float delayAttack;

    [SerializeField] private float rangeSpawnSkill1;

    [SerializeField] private SkillBoss skill1Boss;


    [Header("SKILL 2")]

    [SerializeField] private float skill2AttackRange;

    [SerializeField] private float skill2CoolDown;

    [SerializeField] private float rangeSpawnSkill2;

    [SerializeField] private List<TurnShoot> listTurnShootSkill2;

    [SerializeField] private int idBulletSkill2;

    [SerializeField] private float delayPerTurn;





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
        if (dir.x > 0)
        {
            skill1Boss.transform.localPosition = new Vector3(rangeSpawnSkill1, 0f);

            skill1Boss.gameObject.GetComponent<SpriteRenderer>().flipX = true;

            skill1Boss.gameObject.SetActive(true);
        }
        else
        {
            skill1Boss.transform.localPosition = new Vector3(-rangeSpawnSkill1, 0f);

            skill1Boss.gameObject.GetComponent<SpriteRenderer>().flipX = false;

            skill1Boss.gameObject.SetActive(true);
        }

    }

    public void CastSkill2()
    {

        int turns = Random.Range(2, listTurnShootSkill2.Count + 1);
        StartCoroutine(CastSkill2AllTurn(listTurnShootSkill2.Count));

    }

    IEnumerator CastSkill2AllTurn(int turns)
    {
        for (int turn = 0; turn < turns; turn++)
        {
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
            yield return new WaitForSeconds(delayPerTurn);
        }






    }


    void Update()
    {
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


