using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class BuildableObject : MonoBehaviour
{
    [SerializeField] BuildableData buildableData;

    private float cur_health;

    private SpriteRenderer spriteRenderer;

    private Color defaultColor;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        defaultColor = spriteRenderer.color;
        cur_health = buildableData.Health;

    }

    void OnDamaged(float damage)
    {

        cur_health -= damage;
        if (cur_health <= 0.01f)
        {
            Destroy(gameObject);
        }
        else
        {


            spriteRenderer.DOColor(Color.red, 0.05f).SetLoops(2, LoopType.Yoyo).OnComplete(() => spriteRenderer.color = defaultColor);
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDamaged(100f);
        }
    }

    

    
   
}
