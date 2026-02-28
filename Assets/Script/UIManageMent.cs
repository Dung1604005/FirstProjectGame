using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

[System.Serializable]
public class Pair<T1, T2>
{
    public T1 First;
    public T2 Second;

    public Pair() { }

    public Pair(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;
        
        Pair<T1, T2> other = (Pair<T1, T2>)obj;
        return EqualityComparer<T1>.Default.Equals(First, other.First) &&
               EqualityComparer<T2>.Default.Equals(Second, other.Second);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + (First != null ? First.GetHashCode() : 0);
            hash = hash * 31 + (Second != null ? Second.GetHashCode() : 0);
            return hash;
        }
    }
    
}
public class UIManageMent : MonoBehaviour
{
    public static UIManageMent Instance { get; set; }
    [Header("Add Item")]
    [SerializeField] private PopUpAddedItem popUpAddedItem;
    public PopUpAddedItem PopUpAddedItem => popUpAddedItem;

    private Queue<Pair<ItemData, int>> addedItemQueue = new Queue<Pair<ItemData, int>>();

    [Header("SHOP")]
    [SerializeField] private ShopSystem shopSystem;
    public ShopSystem ShopSystem => shopSystem;
    [SerializeField] private TextMeshProUGUI goldText;
    public TextMeshProUGUI GoldText => goldText;
    [Header("EXPSTAT")]
    [SerializeField] private ExpStatSystemUI expStatSystemUI;
    public ExpStatSystemUI ExpStatSystemUI => expStatSystemUI;
    [Header("Equipment")]
    [SerializeField] private EquipmentSystemUI equipmentSystemUI;
    public EquipmentSystemUI EquipmentSystemUI => equipmentSystemUI;

    [Header("Inventory")]
    [SerializeField] private InventoryUI inventoryUI;
    public InventoryUI InventoryUI => inventoryUI;


    [Header("HEALTH")]
    [SerializeField] private TextMeshProUGUI warning;

    [SerializeField] private Image hpBar;

    [SerializeField] private float fillTargetHp;
    [Header("EXP")]
    
    [SerializeField] private TextMeshProUGUI levelText;
    public TextMeshProUGUI LevelText=> levelText;
    [SerializeField] private Image expBar;
    
    [SerializeField] private float fillTargetExp;
    [SerializeField] private float fillSpeed;

    [Header("BULLETUI")]
    [SerializeField] private BulletUIController bulletUIController;
    public BulletUIController BulletUIController => bulletUIController;

    [SerializeField] private TextMeshProUGUI reloadingText;
    public TextMeshProUGUI ReloadingText => reloadingText;

    [Header("Dialogue")]

    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;

    [Header("Quest")]

    [SerializeField] private QuestUI questUI;
    public QuestUI QuestUI => questUI;


    [Header("Scene Loading")]

    [SerializeField] private LoadingAdditive loadingAdditive;

    public LoadingAdditive LoadingAdditive => loadingAdditive;


    [Header("OTHER")]
    [SerializeField] private Sprite tabBackground_Active;
    public Sprite TabBackground_Active => tabBackground_Active;
    [SerializeField] private Sprite tabBackground_Inactive;
    public Sprite TabBackground_Inactive => tabBackground_Inactive;

    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration;

    private Coroutine flashRoutine;


    public void TurnOnReloadingText()
    {
        reloadingText.gameObject.SetActive(true);
    }
    public void TurnOffReloadingText()
    {
        reloadingText.gameObject.SetActive(false);
    }
    //Canh bao
    public void UpdateWarning(string content)
    {
        warning.text = content;
    }
    public void TurnOnWarning()
    {
        Debug.Log("WARNING");
        warning.gameObject.SetActive(true);
        warning.DOKill();
        warning.DOFade(0f, 2f).OnComplete(() => { warning.gameObject.SetActive(false); warning.alpha = 1f; });

    }

    // Cap nhat thanh mau
    public void SetHealthBar(float hp, float mx)
    {

        fillTargetHp = hp / mx;

    }
    public void SetExpBar(float exp, float mx)
    {
        fillTargetExp = exp / mx;
        String text = GameManageMent.Instance.PlayerManager.ExpSystem.Lv.ToString();
        if(GameManageMent.Instance.PlayerManager.ExpSystem.Lv < 10){
            text = "0" + text;
        } 
        levelText.text = text;
    }
    public void SetGoldText(string text)
    {
        goldText.text = text;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(this);
        
        inventoryUI.TurnOff();
        expStatSystemUI.TurnOff();
        ShopSystem.TurnOff();
        dialogueUI.Init();

    }
    public void DoFadeIn(Image image, float duration)
    {
        image.DOFade(0f, duration);
    }
    public void DoFadeOut(Image image, float duration)
    {
        image.DOFade(1f, duration);
    }

    public void AddItemToQueue(ItemData itemData, int mount)
    {

        if (popUpAddedItem.State == true && itemData.ItemName == popUpAddedItem.Name.text && itemData.MaxStack >= mount + popUpAddedItem.Stock)
        {
            popUpAddedItem.UpdateAmount(mount);
        }
        else
        {

            addedItemQueue.Enqueue(new Pair<ItemData, int>(itemData, mount));
        }


    }
    

    void Start()
    {
        // Khi OnHealh duoc goi thi sethealth cung duoc goi

    }
    int c = 0;

    // Update is called once per frame
    void Update()
    {
        // Dich chuyen thanh mau
        if (hpBar.fillAmount != fillTargetHp)
        {
            hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, fillTargetHp, fillSpeed * Time.deltaTime);
        }
        if (expBar.fillAmount != fillTargetExp)
        {
            expBar.fillAmount = Mathf.Lerp(expBar.fillAmount, fillTargetExp, fillSpeed * Time.deltaTime);
        }

        if (addedItemQueue.Count > 0 && popUpAddedItem.PopUpAddedItemImage.color.a <= 0.000001f)
        {
            c += 1;

            popUpAddedItem.SetInfo(addedItemQueue.Peek().First.Icon, addedItemQueue.Peek().Second, addedItemQueue.Peek().First.ItemName);
            addedItemQueue.Dequeue();

        }

    }
}
