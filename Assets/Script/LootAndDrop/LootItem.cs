using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Rendering;
using UnityEngine;

public class LootItem : MonoBehaviour, IPoolable
{
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float defaultY;
    [SerializeField] private int indexItem;
    public int IndexItem => indexItem;
    [SerializeField] private float offSet;

    [SerializeField] private float rangePick;

    [SerializeField] private float moveSpeed;

    [SerializeField] private int amount;

    public int Amount => amount;

    private float timer = 0f;

    private bool isHover = false;

    private bool isDropping = true;
    [SerializeField] private float jumpHeight = 2f;

    [SerializeField] private float timeJump;

    [SerializeField] private AnimationCurve bounceCurve;

    [SerializeField] private float offSetDrop;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        bounceCurve.postWrapMode = WrapMode.PingPong;
        bounceCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
    }

    public void OnDeSpawn()
    {
        isDropping = true;
    }
    IEnumerator ItemDrop(Vector2 start, Vector2 end)
    {
        float t = 0f;
        while (t < timeJump)
        {
            t += Time.deltaTime;
            float h = bounceCurve.Evaluate(t/timeJump) * jumpHeight;
            Vector2 pos = Vector2.Lerp(start, end, t/timeJump);
            pos.y += h;
            transform.position = pos;
            yield return null;
        }
        isDropping = false;
        defaultY = transform.position.y;
    
    }
    public void OnSpawn()
    {
        timer = 0f;
        isHover = false;
        
    }
    public void SetInfo(int index, int _amount)
    {
        indexItem = index;
        spriteRenderer.sprite = GameManageMent.Instance.ItemDataBase.ItemDatas[index].Icon;
        amount = _amount;
        float offsetX = UnityEngine.Random.Range(-offSetDrop, offSetDrop);
        float offsetY = UnityEngine.Random.Range(-offSetDrop, offSetDrop);
        Vector2 dropPos = transform.position;
        dropPos.x += offsetX;
        dropPos.y += offsetY;
        
        StartCoroutine(ItemDrop(transform.position, dropPos));
        

    }
    public void SetStateHover(bool state)
    {
        isHover = state;
        if (state == true)
        {
            transform.DOScaleX(1.5f, 0.2f);
            transform.DOScaleY(1.5f, 0.2f);
        }
        else
        {
            transform.DOScaleX(1f, 0.2f);
            transform.DOScaleY(1f, 0.2f);
        }
    }
    public ItemData GetItemData()
    {
        return GameManageMent.Instance.ItemDataBase.ItemDatas[IndexItem];
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
    public void AutoPick()
    {
        Vector2 pos = transform.position;
        float range = (GameManageMent.Instance.PlayerManager.PlayerController.getPos() - pos).sqrMagnitude;



        if (GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].Type == ItemType.Material||GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].Type == ItemType.Bullet)
        {

            AddToPlayer((GameManageMent.Instance.PlayerManager.PlayerController.getPos() - pos).normalized);
            if (range <= 3f)
            {
                if (GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].Type == ItemType.Bullet)
                {
                    GameManageMent.Instance.PlayerManager.AddBullet(GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].ItemName, amount);
                    GameManageMent.Instance.PoolManager.LootPool.DeSpawn(this);
                }
                else
                {
                    if(UIManageMent.Instance.InventoryUI.Inven.TryAdd(GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem], amount))
                    {
                        UIManageMent.Instance.InventoryUI.Inven.Add(GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem], amount);
                        GameManageMent.Instance.PoolManager.LootPool.DeSpawn(this);
                    }
                    
                }
                
            }
        }
        
    }
    public void PickUp()
    {
        Vector2 pos = transform.position;
        float range = (GameManageMent.Instance.PlayerManager.PlayerController.getPos() - pos).sqrMagnitude;
        if (GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].Type != ItemType.Material && GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].Type != ItemType.Bullet)
        {
            if (range <= rangePick*rangePick)
            {
                if(UIManageMent.Instance.InventoryUI.Inven.TryAdd(GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem], amount))
                {
                    UIManageMent.Instance.InventoryUI.Inven.Add(GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem], amount);
                    GameManageMent.Instance.PoolManager.LootPool.DeSpawn(this);
                }
                
            }
        }
    }

    void Update()
    {
        if (isDropping)
        {
            return;
        }
        timer += Time.deltaTime;
        timer %= 1f;
        Vector2 pos = transform.position;
        float range = (GameManageMent.Instance.PlayerManager.PlayerController.getPos() - pos).sqrMagnitude;
        if (range > rangePick * rangePick)
        {
            if (!isHover)
            {
                Floating();
            }
            

        }
        else
        {
            if ((GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].Type == ItemType.Material||GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].Type == ItemType.Bullet) &&
            UIManageMent.Instance.InventoryUI.Inven.TryAdd(GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem], amount))
            {
                AutoPick();
                
            }
            else
            {
                Floating();
            }
        }
    }
}
