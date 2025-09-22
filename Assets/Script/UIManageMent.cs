using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
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

    [SerializeField] private Health healthPlayer;
    [SerializeField] private Image hpBar;

    [SerializeField] private float fillTargetHp;
    [Header("EXP")]
    [SerializeField] private Image expBar;
    [SerializeField] private ExpSystem expSystem;
    [SerializeField] private float fillTargetExp;
    [SerializeField] private float fillSpeed;

    [Header("BULLETUI")]
    [SerializeField] private BulletUIController bulletUIController;
    public BulletUIController BulletUIController => bulletUIController;

    [SerializeField] private TextMeshProUGUI reloadingText;
    public TextMeshProUGUI ReloadingText => reloadingText;

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
        healthPlayer.OnHealthChanged.AddListener(SetHealthBar);
        expSystem.OnExpChange.AddListener(SetExpBar);
        inventoryUI.TurnOff();
        expStatSystemUI.TurnOff();
        ShopSystem.TurnOff();
        
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
    private IEnumerator FlashRoutine(SpriteRenderer renderer, Material defaultMaterial)
    {
        renderer.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        if(renderer!= null){

            renderer.material = defaultMaterial;
        }
        
        
    }

    public void Flash(SpriteRenderer spriteRenderer)
    {
        Material defaultMaterial = spriteRenderer.material;
        StartCoroutine(FlashRoutine(spriteRenderer, defaultMaterial));

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
            Debug.Log(c);
            popUpAddedItem.SetInfo(addedItemQueue.Peek().First.Icon, addedItemQueue.Peek().Second, addedItemQueue.Peek().First.ItemName);
            addedItemQueue.Dequeue();

        }

    }
}
