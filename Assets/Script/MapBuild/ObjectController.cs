using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField] private int indexLootTable;
    [SerializeField] private int amountDrop;

    [SerializeField] private float maxHealth;
    private Coroutine flashRoutine;


    private float curHealth;

    

    public void OnDamaged(float damaged)
    {
        curHealth -= damaged;
        GameManageMent.Instance.EffectController.Flash(this.GetComponent<SpriteRenderer>(), ref flashRoutine);
        if (curHealth <= 0f)
        {
            GameManageMent.Instance.DropSystem.DropItem(indexLootTable, amountDrop, this.gameObject.transform.position);
            Destroy(this.gameObject);
        }
    }
    
    void Awake()
    {
        curHealth = maxHealth;

    }



    

    

}
