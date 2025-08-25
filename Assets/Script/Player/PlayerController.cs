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
    [SerializeField] private Weapon weapon;

    private GameObject weaponPrefab;


    private StatPlayer stat;
    public StatPlayer Stat => stat;

    private ExpSystem expSystem;
    public ExpSystem ExpSystem => expSystem;

    private Health health;
    public Health Health => health;
    public static PlayerController Instance { get; private set; }
    private Rigidbody2D rb;
    private bool usingGun = false;
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
            float dam = collision.gameObject.GetComponentInParent<EnemyMelee>().GetDame();
            this.GetComponent<Health>().OnDamaged(dam);
        }

    }

    // Tao anim cho Blend tree
    public void EquipGunAnim()
    {
        usingGun = true;
        anim.SetBool("UsingGun", usingGun);
    }
    public void UnEquipGunAnim()
    {
        usingGun = false;
        anim.SetBool("UsingGun", usingGun);
    }
    public void AnimUpdate(float x, float y)
    {
        anim.SetFloat("MoveX", x);
        anim.SetFloat("MoveY", y);
    }
    public void UpdatePunchAnim()
    {
        punching = true;
        Vector2 dir = GetDirFromMouseToPlayer();
        float angle = Mathf.Atan2(dir.y, dir.x);
        float y = Mathf.Sin(angle);
        float x = Mathf.Cos(angle);


        Debug.Log(x + " " + y);
        anim.SetTrigger("Punch");
        AnimUpdate(x, y);
    }
    public void EndPunch()
    {
        punching = false;
    }
    public Vector2 GetDirFromMouseToPlayer()
    {
        Vector2 playerPos = getPos();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = (mousePos - playerPos).normalized;
        return dir;
    }
    //Di chuyen
    void Move()
    {
        Debug.Log("MOVE");
        float movex = Input.GetAxis("Horizontal");
        float movey = Input.GetAxis("Vertical");
        if (weapon == null)
        {
            if (punching == false)
            {
                AnimUpdate(movex, movey);
            }
        }
        else
        {
            if (weapon.Attacking == false)
            {
                if (usingGun)
                {
                    weapon.UpdateAnim(movex, movey);
                }
                AnimUpdate(movex, movey);
            }
        }





        Vector2 dir = new Vector2(movex, movey).normalized;
        Vector2 new_pos = rb.position + dir * Time.fixedDeltaTime * stat.Speed;
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
            Vector2 dir = GetDirFromMouseToPlayer();
            if (weapon != null)
            {
                weapon.Attack(dir.x, dir.y);
            }
            else
            {
                UpdatePunchAnim();
            }
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        expSystem = GetComponent<ExpSystem>();
        stat = GetComponent<StatPlayer>();
        health = GetComponent<Health>();
        weapon = null;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }



    }
    void Start()
    {
        health.SetMaxHp(stat.MaxHP, true);

    }
    void FixedUpdate()
    {
        if (GameManageMent.Instance.GameState == GameState.Pause)
        {
            return;
        }
        // Khong cho di chuyen luc swap de weapon cap nhat anim
        if (!swapping)
        {
            Move();

        }
       

    }


    void UpdateCountDown()
    {
        if (weapon == null || weapon.Attacking == false)
        {
            Debug.Log(attackCountDown);
            attackCountDown += Time.deltaTime;
            if (weapon == null)
            {
                if (attackCountDown >= punchCountDown)
                {
                    Attack();
                }
            }
            else
            {
                if (attackCountDown >= weapon.WeaponData.CoolDown)
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
    public void EquipSlot(int slot)
    {

        ItemData itemData = UIManageMent.Instance.EquipmentSystemUI.EquipMentSystem.Slots[slot].ItemData;
        if (weapon != null)
        {
            weapon = null;
            Destroy(weaponPrefab);
        }
        if (itemData == null)
        {
            UnEquipGunAnim();
            weapon = null;
            return;
        }
        
        if (itemData.Type == ItemType.Gun)
        {
            
            EquipGunAnim();
            GunData gunData = itemData as GunData;
           
            weaponPrefab = Instantiate(gunData.Gun.gameObject, this.transform.GetChild(2).transform);
            weapon = weaponPrefab.GetComponent<Weapon>();
            return;
        }
        else
        {
            UnEquipGunAnim();
        }
        if (itemData.Type != ItemType.Melee)
        {
            weapon = null;


        }
        else
        {
            
            MeleeData meleeData = itemData as MeleeData;
            weaponPrefab = Instantiate(meleeData.Melee.gameObject, this.transform.GetChild(2).transform);
            weapon = weaponPrefab.GetComponent<Weapon>();
            return;
        }
        if (itemData.Type == ItemType.Consumable)
        {
            HpPotionData hpPotionData = itemData as HpPotionData;
            Health.OnHeal(hpPotionData.HpRecover);
            UIManageMent.Instance.EquipmentSystemUI.EquipMentSystem.Slots[slot].Remove(1);

        }
    }
    void ChooseSlot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
             EquipSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
             EquipSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EquipSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            EquipSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            EquipSlot(5);
        }
    }

    void Update()
    {
        if (GameManageMent.Instance.GameState == GameState.Pause)
        {
            return;
        }

        UpdateCountDown();
        ChooseSlot();




    }
}
