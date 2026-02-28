using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField] private int indexLootTable;
    [SerializeField] private int amountDrop;

    [SerializeField] private float maxHealth;
    private Coroutine flashRoutine;


    private float curHealth;
    private Material defaultMaterial;

    [SerializeField] private float timeSpawn;

    

    public void OnDamaged(float damaged)
    {
        curHealth -= damaged;
        GameManageMent.Instance.EffectController.Flash(this.GetComponent<SpriteRenderer>(), defaultMaterial, ref flashRoutine);
        if (curHealth <= 0f)
        {
            GameManageMent.Instance.DropSystem.DropItem(indexLootTable, amountDrop, this.gameObject.transform.position);
            StartCoroutine(SpawnActionRoutine());
        }
    }

    IEnumerator SpawnActionRoutine()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        this.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(timeSpawn);

        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        this.GetComponent<SpriteRenderer>().enabled = true;
        curHealth = maxHealth;

    }
    
    void Awake()
    {
        curHealth = maxHealth;
        defaultMaterial = this.GetComponent<SpriteRenderer>().material;

    }



    

    

}
