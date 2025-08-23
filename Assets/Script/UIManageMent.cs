using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class UIManageMent : MonoBehaviour
{
    public static UIManageMent Instance{ get; set; }
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

    [SerializeField] private float fillTarget;
    [SerializeField] private float fillSpeed;

    //Canh bao
    public void UpdateWarning(string content)
    {
        warning.text = content;      
    }
    public void TurnOnWarning()
    {
        warning.gameObject.SetActive(true);
        warning.DOFade(0f, 2f).OnComplete(() => { warning.gameObject.SetActive(false); warning.alpha = 1f; });
        
    }
    
    // Cap nhat thanh mau
    public void SetHealthBar(float hp, float mx)
    {
        fillTarget = hp / mx;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }
    public void DoFadeIn(Image image, float duration)
    {
        image.DOFade(0f, duration);
    }
    public void DoFadeOut(Image image, float duration)
    {
        image.DOFade(1f, duration);
    }
    void Start()
    {
        // Khi OnHealh duoc goi thi sethealth cung duoc goi
        healthPlayer.OnHealthChanged.AddListener(SetHealthBar);
    }


    // Update is called once per frame
    void Update()
    {
        // Dich chuyen thanh mau
        if (hpBar.fillAmount != fillTarget)
        {
            hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, fillTarget, fillSpeed * Time.deltaTime);
        }
    }
}
