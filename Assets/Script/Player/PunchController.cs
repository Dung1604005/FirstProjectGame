using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchController : MonoBehaviour
{


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.DESTROYABLE_OBJECT_TAG)
        {
            try
            {
                 collision.GetComponent<ObjectController>().OnDamaged(GameManageMent.Instance.PlayerManager.Stat.Atk);

            }
            catch(Exception e)
            {
                Debug.LogError("CANNOT DESTROY DESTROYABLE_OBJECT_TAG BY PUNCH");
                throw e;
            }
        }
        else if (collision.tag == GameConfig.HITBOX_ENEMY)
        {

            try
            {
                collision.GetComponentInParent<EnemyBase>().HealthSystem.OnDamaged(GameManageMent.Instance.PlayerManager.Stat.Atk);
            }
            catch(Exception e)
            {
                Debug.LogError("CANNOT ATTACK ENEMY BY PUNCH");
                throw e;
            }
            
        }
    }
}
