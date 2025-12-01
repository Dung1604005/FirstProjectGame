using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRange", menuName = "Script/Enemy/EnemyRange")]
public class EnemyRangeData : EnemyBaseData
{
    [SerializeField] private int indexBullet;
    
    public int IndexBullet => indexBullet;

    [SerializeField] private float radiusBullet;
    public float RadiusBullet => radiusBullet;

}
