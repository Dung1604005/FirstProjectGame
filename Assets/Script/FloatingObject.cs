using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float defaultY;
    [SerializeField] private int indexItem;
    [SerializeField] private float offSet;
    private float timer = 0f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        

    }
    void Start()
    {
        defaultY = transform.position.y;
    }
    public void SetInfo(int index)
    {
        indexItem = index;
        spriteRenderer.sprite = GameManageMent.Instance.ItemDataBase.ItemDatas[index].Icon;

    }
    private void Floating()
    {
        if (timer <= 0.25f)
        {
            animationCurve = AnimationCurve.Linear(0f, defaultY, 0.25f, defaultY - offSet);
        }
        else if (timer <= 0.5f)
        {
            animationCurve = AnimationCurve.Linear(0.25f, defaultY - offSet, 0.5f, defaultY);
        }
        else if (timer <= 0.75f)
        {
            animationCurve = AnimationCurve.Linear(0.5f, defaultY, 0.75f, defaultY + offSet);
        }
        else if (timer <= 1f)
        {
            animationCurve = AnimationCurve.Linear(0.75f, defaultY + offSet, 1f, defaultY);
        }

        Vector3 pos = transform.position;
        pos.y = animationCurve.Evaluate(timer);
        transform.position = pos;
    }

    void Update()
    {
        timer += Time.deltaTime;
        timer %= 1f;
        Floating();

        
        //Debug.Log(transform.position.y);
        
        
        
    }
}
