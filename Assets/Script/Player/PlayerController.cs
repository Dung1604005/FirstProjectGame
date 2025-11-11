using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
 
    private SlotPlayerController slotPlayerController;
    public SlotPlayerController SlotPlayerController => slotPlayerController;

    private LootSystem lootSystem;
    public LootSystem LootSystem=> lootSystem;
    
    private Rigidbody2D rb;
    private bool usingWeapon = false;
    private bool punching = false;

    private bool swapping = false;

    private float attackCountDown = 0f;

    [SerializeField] private float punchCountDown;

    private Animator anim;
    // Kiem soat va cham
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "AttackMeleeEnemyHitBox")
        {
            float dam = collision.gameObject.GetComponentInParent<EnemyMelee>().GetDamage();
            this.GetComponent<Health>().OnDamaged(dam);
        }
    
    }

    public void OllisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
    }


    // Tao anim cho Blend tree
    public void EquipWeaponAnim()
    {
        usingWeapon = true;
        anim.SetBool(GameConfig.USINGWEAPON_BOOL, usingWeapon);
    }
    public void UnEquipWeaponAnim()
    {
        usingWeapon = false;
        anim.SetBool(GameConfig.USINGWEAPON_BOOL, usingWeapon);
    }
    public void AnimUpdate(float x, float y)
    {
        
        Vector2 a = new Vector2(x, y).normalized;
        anim.SetFloat(GameConfig.MOVEX_FLOAT, x);
        anim.SetFloat(GameConfig.MOVEY_FLOAT, y);
        anim.SetFloat(GameConfig.SPEED_PARAMETER, a.sqrMagnitude);
    }
    public void UpdatePunchAnim()
    {
        punching = true;
        Vector2 dir = GameManageMent.Instance.PlayerManager.GetDirFromMouseToPlayer();
        float angle = Mathf.Atan2(dir.y, dir.x);
        float y = Mathf.Sin(angle);
        float x = Mathf.Cos(angle);


        
        anim.SetTrigger(GameConfig.PUNCH_TRIGGER);
        AnimUpdate(x, y);
    }
    
    public void EndPunch()
    {
        punching = false;
    }
    public void InteractNpc()
    {
        LayerMask npcMask = LayerMask.GetMask(GameConfig.NPC_MASK);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos, npcMask);

        if (hit != null)
        {
            if (GameManageMent.Instance.Interacting == false)
            {
                GameManageMent.Instance.SetCurSorInteract();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                hit.gameObject.GetComponent<NPC>().TurnOnInteract();
                GameManageMent.Instance.SetNpcInteracting(hit.gameObject.GetComponent<NPC>());
            }

        }
        else
        {
            if (GameManageMent.Instance.Interacting == true)
            {
                GameManageMent.Instance.SetCurSorNormal();
                GameManageMent.Instance.SetNpcInteracting(null);
            }
        }
    }
    
    //Di chuyen
    void Move()
    {
        if(punching || (usingWeapon && slotPlayerController.Weapon.Attacking)){
            return;
        }
        float movex = Input.GetAxis(GameConfig.HORIZONTAL);
        float movey = Input.GetAxis(GameConfig.VERTICAL);
        if (slotPlayerController.Weapon == null)
        {
            if (punching == false)
            {
                AnimUpdate(movex, movey);
            }
        }
        else
        {
            if (slotPlayerController.Weapon.Attacking == false)
            {
                if (usingWeapon)
                {
                    if (slotPlayerController.Weapon.WeaponData.Type == ItemType.Gun)
                    {
                        slotPlayerController.Weapon.UpdateAnim(movex, movey);
                    }
                    
                }
                AnimUpdate(movex, movey);
            }
        }

        Vector2 dir = new Vector2(movex, movey).normalized;
        Vector2 new_pos = rb.position + dir * Time.fixedDeltaTime * GameManageMent.Instance.PlayerManager.Stat.Speed;
        rb.MovePosition(new_pos);
    }
    // Lay vi tri
    public Vector2 getPos()
    {
        //Debug.Log(rb.position.x + ", " + rb.position.y);
        return rb.position;
    }
    // Ban
    void Attack()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 dir = GameManageMent.Instance.PlayerManager.GetDirFromMouseToPlayer();
            if (slotPlayerController.Weapon != null)
            {
                slotPlayerController.Weapon.Attack(dir.x, dir.y);
            }
            else
            {
                UpdatePunchAnim();
            }
            attackCountDown = 0f;
        }
    }
     void UpdateCountDown()
    {
        if (slotPlayerController.Weapon == null || slotPlayerController.Weapon.Attacking == false)
        {
            
            attackCountDown += Time.deltaTime;
            if (slotPlayerController.Weapon  == null)
            {
                if (attackCountDown >= punchCountDown)
                {
                    Attack();
                }
            }
            else
            {
                if (attackCountDown >= slotPlayerController.Weapon.WeaponData.CoolDown)
                {
                    Attack();
                }
            }
        }
        else
        {
            attackCountDown = 0f;
        }
    }



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        slotPlayerController = GetComponent<SlotPlayerController>();
        lootSystem = GetComponent<LootSystem>();
    }
    
    void FixedUpdate()
    {
        if (GameManageMent.Instance.GameState == GameState.Pause)
        {
            return;
        }
        // Khong cho di chuyen luc swap de weapon cap nhat anim
        // if (!swapping)
        // {
        //     Move();

        // }
       

    }


    void Update()
    {
        if (GameManageMent.Instance.GameState == GameState.Pause)
        {
            return;
        }
        if (!swapping)
        {
            Move();

        }
        InteractNpc();
        UpdateCountDown();
        slotPlayerController.ChooseSlot();
        lootSystem.CheckHoverItem();
        if (GameManageMent.Instance.BuildManager.BuildMode && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (GameManageMent.Instance.BuildManager.BuildPlacement.CanPlace())
            {
                GameManageMent.Instance.BuildManager.BuildPlacement.PlaceObject();
                UIManageMent.Instance.EquipmentSystemUI.EquipMentSystem.UseSlot(slotPlayerController.CurSlotEquip, 1);
                if (UIManageMent.Instance.EquipmentSystemUI.EquipMentSystem.Slots[slotPlayerController.CurSlotEquip].Count == 0)
                {
                    GameManageMent.Instance.BuildManager.TurnOffBuildMode();
                }

            }

        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(slotPlayerController.Weapon != null && slotPlayerController.Weapon.WeaponData.Type == ItemType.Gun)
            {
                (slotPlayerController.Weapon as Gun).Reload();
            }
            
        }




    }
}
