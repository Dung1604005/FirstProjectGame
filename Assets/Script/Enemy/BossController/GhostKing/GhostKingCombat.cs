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

    [SerializeField] private float skill2Damage;

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
        if(dir.x > 0)
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

    public void CastSkill2(int turns)
    {
        
        TurnShoot turnShoot = listTurnShootSkill2[0];
        StartCoroutine(CastSkill2OneTurn(turnShoot.amount, turnShoot.first_Angle, turnShoot.delayAttack, turnShoot.speedBullet, 0, turns - 1));
        
    }
    
    IEnumerator CastSkill2OneTurn(int amount, float first_Angle, float delayAttack, float speed, int curTurn, int endTurn)
    {
        
        float angleStep = 360/amount;

        float currentAngle = first_Angle;

        for(int i = 0;i < amount; i++)
        {
            float dirX = Mathf.Cos(currentAngle*Mathf.Deg2Rad);

            float dirY = Mathf.Sin(currentAngle*Mathf.Deg2Rad);

            Vector2 dir = new Vector2(dirX, dirY).normalized;

            Vector2 posSpawn = (Vector2)this.transform.position + dir*rangeSpawnSkill2;
            BulletController bullet = GameManageMent.Instance.PoolManager.BulletPoolsList[idBulletSkill2].Spawn(posSpawn);


            if(bullet != null)
            {
                bullet.SetInfo(skill2Damage, idBulletSkill2);
                bullet.SetSpeed(speed);

                bullet.Fire(dir);
                yield return new WaitForSeconds(delayAttack);           
            }
            else
            {
                Debug.LogError("CANNOT SPAWN BULLET FROM BOSS");
            }
            currentAngle += angleStep;

        }


        yield return new WaitForSeconds(delayPerTurn);

        if(curTurn < endTurn)
        {
            TurnShoot turnShoot = listTurnShootSkill2[curTurn + 1];
            StartCoroutine(CastSkill2OneTurn(turnShoot.amount, turnShoot.first_Angle, turnShoot.delayAttack, turnShoot.speedBullet, curTurn + 1, endTurn));
        }
        
        
    }

    
}


