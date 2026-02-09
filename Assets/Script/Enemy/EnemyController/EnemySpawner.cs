using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SenderEvent
{
    [SerializeField] private List<EnemyBaseData> enemyBaseDataList;

    [SerializeField] private List<EnemyBase> enemyCurrentList;

    [SerializeField] private float spawnCoolDown;

    [SerializeField] private float checkCoolDown;

    private float checkTimer = 0f;

    private float cooldownTimer = 0f;



    void Awake()
    {
        enemyCurrentList = new List<EnemyBase>();
        
    }

    private void CheckCondition()
    {

        if (enemyCurrentList.Count != enemyBaseDataList.Count)
        {
            return;
        }
        for (int i = 0; i < enemyCurrentList.Count; i++)
        {
            if (enemyCurrentList[i].gameObject.activeInHierarchy)
            {

                return;
            }
        }

        SendEvent();
        if (eventSended)
        {
            
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (enemyCurrentList.Count == enemyBaseDataList.Count || eventSended)
        {
            checkTimer += Time.deltaTime;
            if (checkTimer >= checkCoolDown)
            {
                checkTimer = 0f;
                CheckCondition();
            }
            return;
        }
        cooldownTimer += Time.deltaTime;
        checkTimer += Time.deltaTime;
        if (cooldownTimer >= spawnCoolDown)
        {
            cooldownTimer = 0f;
            int indexNextEnemySpawn = enemyBaseDataList[enemyCurrentList.Count].IndexEnemy;
            EnemyBase enemyBase = GameManageMent.Instance.PoolManager.EnemytPoolsList[indexNextEnemySpawn].Spawn(transform.position);
            enemyCurrentList.Add(enemyBase);
        }
        if (checkTimer >= checkCoolDown)
        {
            checkTimer = 0f;
            CheckCondition();
        }

    }


}
