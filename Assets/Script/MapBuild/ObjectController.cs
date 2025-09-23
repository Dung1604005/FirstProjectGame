using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField] private int indexLootTable;
    [SerializeField] private int amountDrop;

    [SerializeField] private float maxHealth;


    private float curHealth;

    

    public void OnDamaged(float damaged)
    {
        curHealth -= damaged;
        UIManageMent.Instance.Flash(this.GetComponent<SpriteRenderer>());
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
