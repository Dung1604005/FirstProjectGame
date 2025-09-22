using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchController : MonoBehaviour
{


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.DESTROYABLE_OBJECT_TAG)
        {
            collision.GetComponent<ObjectController>().OnDamaged(GameManageMent.Instance.PlayerManager.Stat.Atk);
        }
    }
}
