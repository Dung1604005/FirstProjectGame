using System;
using System.Collections;
using System.Collections.Generic;
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
        anim.SetFloat("DirX", dirX);
        anim.SetFloat("DirY", dirY);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.HITBOX_ENEMY)
        {
            collision.gameObject.GetComponentInParent<Health>().OnDamaged(weaponData.Damaged + PlayerController.Instance.Stat.Atk);
        }
    }
    
    public override void Attack(float dirX, float dirY)
    {
        
        float angle = Mathf.Atan2(dirY, dirX);
        float y = Mathf.Sin(angle);
        float x = Mathf.Cos(angle);
        attacking = true;
        PlayerController.Instance.AnimUpdate(x, y);
        UpdateAnim(x, y);

    }
    
}
