using System.Collections;
using UnityEngine;

public class HuyCombat : MonoBehaviour
{
    [Header("SKILL 1")]

    [SerializeField] private float skill1Damage;

    [SerializeField] private float skill1AttackRange;

    public float Skill1AttackRange => skill1AttackRange;

    [SerializeField] private float skill1CoolDown;

    public float Skill1CoolDown => skill1CoolDown;

    [SerializeField] private float delayAttackSkill1;

    [SerializeField] private Vector2 rangeSpawnSkill1;

    [SerializeField] private SkillBoss skill1Boss;
    
    [SerializeField] private bool unlockedSkill1;

    public bool UnlockedSkill1 => unlockedSkill1;

    void Init()
    {
        skill1Boss.SetDamage(skill1Damage);
        
    }

    void Start()
    {
        Init();
    }


    public void CastSkill1(Vector2 dir)
    {
        StartCoroutine(CastSkill1Coroutine(dir));

    }
    IEnumerator CastSkill1Coroutine(Vector2 dir)
    {
        
        yield return new WaitForSeconds(delayAttackSkill1);
        DirType dirType = GameManageMent.Instance.CalculateDirType(dir.x, dir.y);
        if (dirType == DirType.RIGHT)
        {
            skill1Boss.transform.localPosition = new Vector3(rangeSpawnSkill1.x, 0f);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            skill1Boss.gameObject.transform.localScale = new Vector3(-1, 1, 0);

        }
        else if (dirType == DirType.LEFT)
        {
            skill1Boss.transform.localPosition = new Vector3(-rangeSpawnSkill1.x, 0f);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            skill1Boss.gameObject.transform.localScale = new Vector3(1, 1, 0);
        }
        else if (dirType == DirType.DOWN)
        {
            skill1Boss.transform.localPosition = new Vector3(0, -rangeSpawnSkill1.y);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

            skill1Boss.gameObject.transform.localScale = new Vector3(1, 1, 0);
        }
        else
        {
            skill1Boss.transform.localPosition = new Vector3(0, rangeSpawnSkill1.y);
            skill1Boss.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            skill1Boss.gameObject.transform.localScale = new Vector3(-1, 1, 0);

            
        }

        skill1Boss.gameObject.SetActive(true);
       
    }
    void Update()
    {
        // Test
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (GameObject.FindGameObjectWithTag(GameConfig.PLAYER_TAG0) != null)
            {


            }
            CastSkill1((GameManageMent.Instance.PlayerManager.PlayerController.getPos() - (Vector2)this.transform.position).normalized);
        }

        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     CastSkill2();
        // }
        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     if (GameObject.FindGameObjectWithTag(GameConfig.PLAYER_TAG0) != null)
        //     {
        //         CastSkill3(GameManageMent.Instance.PlayerManager.PlayerController.getPos());
        //     }

        // }
    }
}
