using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Rendering;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float defaultY;
    [SerializeField] private int indexItem;
    [SerializeField] private float offSet;

    [SerializeField] private float rangePick;

    [SerializeField] private float moveSpeed;

    [SerializeField] private int amount;

    private float timer = 0f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();


    }
    void Start()
    {
        defaultY = transform.position.y;
    }
    public void SetInfo(int index, int _amount)
    {
        indexItem = index;
        spriteRenderer.sprite = GameManageMent.Instance.ItemDataBase.ItemDatas[index].Icon;
        amount = _amount;

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

    public void AddToPlayer(Vector2 dir)
    {
        
        this.transform.Translate(dir * Time.deltaTime * moveSpeed);
    }
    public void OnPick()
    {
        Vector2 pos = transform.position;
        float range = (PlayerController.Instance.getPos() - pos).sqrMagnitude;
        


        if (GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].Type == ItemType.Material)
        {

            AddToPlayer((PlayerController.Instance.getPos() - pos).normalized);
            if (range <= 3f)
            {
                UIManageMent.Instance.InventoryUI.Inven.Add(GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem], amount);
                Destroy(gameObject);
            }
        }
        else
        {
            
        }
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        timer %= 1f;
        Vector2 pos = transform.position;
        float range = (PlayerController.Instance.getPos() - pos).sqrMagnitude;
        if (range > rangePick * rangePick)
        {
            Floating();
        }
        else
        {
            if (UIManageMent.Instance.InventoryUI.Inven.TryAdd(GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem], amount))
            {
                OnPick();
            }
            else
            {
                Floating();
            }
        }
        






    }
}
