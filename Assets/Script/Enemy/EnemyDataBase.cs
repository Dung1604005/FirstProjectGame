using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataBase", menuName = "Script/EnemyDataBase", order = 1)]
public class EnemyDataBase : ScriptableObject
{
    [SerializeField] private List<EnemyBaseData> enemyDataList = new List<EnemyBaseData>();
    public List<EnemyBaseData> EnemyDataList => enemyDataList;
}
