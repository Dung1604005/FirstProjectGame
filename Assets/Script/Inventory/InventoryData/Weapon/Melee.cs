using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class Melee : Weapon
{
    void Awake()
    {
        anim = GetComponent<Animator>();
        attacking = false;
    }
    public override void UpdateAnim(float dirX, float dirY)
    {
        anim.SetTrigger("isAttack");
        if(Mathf.Abs(dirX) + Mathf.Abs(dirY) > 0)
        {
            anim.SetFloat("DirX", dirX);
            anim.SetFloat("DirY", dirY);
        }
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.HITBOX_ENEMY)
        {
            float damaged = weaponData.Damaged ;
            bool isCrit = GameManageMent.Instance.PlayerManager.CalculateCritDamage(ref damaged);
            collision.gameObject.GetComponentInParent<HealthEnemy>().OnDamaged(weaponData.Damaged , isCrit);
        }
    }
    
    public override void Attack(float dirX, float dirY)
    {
        
        float angle = Mathf.Atan2(dirY, dirX);
        float y = Mathf.Sin(angle);
        float x = Mathf.Cos(angle);
        attacking = true;

        Debug.Log(x + " " + y);
        GameManageMent.Instance.PlayerManager.PlayerController.AnimUpdate(x, y);
        GameManageMent.Instance.PlayerManager.PlayerController.UpdatePlayerDir(x, y);
        UpdateAnim(x, y);
        

    }
    
}
