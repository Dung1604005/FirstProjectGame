using System;
using System.Collections;
using UnityEngine;

public class GroundZoneController : SkillBoss
{
    [SerializeField] private float delayAttack;
    private Animator animator;

    void Init()
    {
        animator = GetComponent<Animator>();
    }
    void Awake()
    {
        Init();
    }
    public void SetActive(bool isActive)
    {
        if (isActive)
        {
            this.gameObject.SetActive(true);
            animator.SetTrigger("start");
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
    public void CastSkill()
    {
        StartCoroutine(CastSkillCoroutine());
    }
    
    IEnumerator CastSkillCoroutine()
    {
        yield return new WaitForSeconds(delayAttack);
        animator.SetTrigger("attack");
    }

}
