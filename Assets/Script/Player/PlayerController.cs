
using UnityEngine;



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

    private float moveX;
    public float MoveX => moveX;

    private float moveY;
    public float MoveY => moveY;


    private Vector2 playerDir;
    public Vector2 PlayerDir => playerDir;

    [SerializeField] private GameObject lightObject;

    public void TurnOnLight()
    {
        if(lightObject != null)
        {
            lightObject.SetActive(true);
        }
        else
        {
            Debug.Log("NO LIGHT");
        }
        
    }
    public void TurnOffLight()
    {

        if(lightObject != null)
        {
            lightObject.SetActive(false);
        }
        else
        {
            Debug.Log("NO LIGHT");
        }
        
    }

    
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
        float speed = a.sqrMagnitude;
        if(speed > 0)
        {
            anim.SetFloat(GameConfig.MOVEX_FLOAT, x);
            anim.SetFloat(GameConfig.MOVEY_FLOAT, y);
        }
        anim.SetFloat(GameConfig.SPEED_PARAMETER, speed);
        
    }
    public void UpdatePunchAnim()
    {
        punching = true;
        Vector2 dir = GameManageMent.Instance.PlayerManager.GetDirFromMouseToPlayer();
        float angle = Mathf.Atan2(dir.y, dir.x);
        float y = Mathf.Sin(angle);
        float x = Mathf.Cos(angle);


        
        anim.SetTrigger(GameConfig.PUNCH_TRIGGER);
        AudioManager.Instance.PlayPunch();
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
    public void UpdatePlayerDir(float movex, float movey)
    {
        Vector2 dir = new Vector2(movex, movey);
        if(dir.sqrMagnitude > 0)
        {
            playerDir = dir;
            
        }
        


    }
    void GetInputMove()
    {
        float movex = Input.GetAxis(GameConfig.HORIZONTAL);
        float movey = Input.GetAxis(GameConfig.VERTICAL);
        moveX = movex;
        moveY = movey;
        UpdatePlayerDir(moveX, moveY);
        

    }
    public void SetPosition(Vector3 pos)
    {
        this.gameObject.transform.position = pos;
    }
    //Di chuyen
    void Move()
    {
        if(punching || (usingWeapon && slotPlayerController.Weapon.Attacking)){
            return;
        }
        
        if (slotPlayerController.Weapon == null)
        {
            if (punching == false)
            {
                AnimUpdate(moveX, moveY);
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
                        slotPlayerController.Weapon.UpdateAnim(moveX, moveY);
                    }
                    
                }
                AnimUpdate(moveX, moveY);   
            }
        }

        Vector2 dir = new Vector2(moveX, moveY).normalized;
        Vector2 new_pos = rb.position + dir * Time.fixedDeltaTime * GameManageMent.Instance.PlayerManager.Stat.Speed;
        //AudioManager.Instance.PlayFootstep();
        rb.MovePosition(new_pos);
    }
    // Lay vi tri
    public Vector2 getPos()
    {
        if(rb == null)
        {
            return Vector2.zero;
        }
        //Debug.Log(rb.position.x + ", " + rb.position.y);
        return rb.position;
    }
    // Ban
    void Attack()
    {
        if (Input.GetKey(KeyCode.Mouse0) && slotPlayerController.Weapon != null && slotPlayerController.Weapon.WeaponData.Type == ItemType.Gun)
        {
            Vector2 dir = GameManageMent.Instance.PlayerManager.GetDirFromMouseToPlayer();
            
            attackCountDown = 0f;
            slotPlayerController.Weapon.Attack(dir.x, dir.y);
            
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 dir = GameManageMent.Instance.PlayerManager.GetDirFromMouseToPlayer();
            if (slotPlayerController.Weapon != null)
            {
                slotPlayerController.Weapon.Attack(dir.x, dir.y);
            }
            else
            {
                
                UpdatePunchAnim();
                UpdatePlayerDir(dir.x, dir.y);
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
    public void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        slotPlayerController = GetComponent<SlotPlayerController>();
        lootSystem = GetComponent<LootSystem>();
        moveX = 0f;
        moveY = 0f;
        playerDir = Vector2.zero;
    }


    void Awake()
    {
        Init();
        GameManageMent.Instance.PlayerManager.SetPlayerComponent(this.GetComponent<PlayerController>(), 
        this.GetComponent<ExpSystem>(), this.GetComponent<GoldPlayer>(), this.GetComponent<StatPlayer>(),
        this.GetComponent<Health>());
    }
    void Start()
    {
        
        this.GetComponent<Health>().OnHealthChanged.AddListener(UIManageMent.Instance.SetHealthBar);
        this.GetComponent<ExpSystem>().OnExpChange.AddListener(UIManageMent.Instance.SetExpBar);
        GameManageMent.Instance.TimeManager.ChangeToMidNight += TurnOnLight;
        GameManageMent.Instance.TimeManager.ChangeToDay += TurnOffLight;
        GameManageMent.Instance.CinemachineVirtualCamera.Follow = this.transform;
    }
    
    void FixedUpdate()
    {
        
        GetInputMove();
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
