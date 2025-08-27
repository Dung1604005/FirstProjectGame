using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    [SerializeField] protected WeaponData weaponData;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected bool attacking;
    public bool Attacking => attacking;

    public WeaponData WeaponData => weaponData;

    [SerializeField] protected Animator anim;
    public void EndAttack()
    {
        
        attacking = false;
    }
    
     public virtual void UpdateAnim(float dirX, float dirY)
    {



    }
    public virtual void Attack(float dirX, float dirY)
    {


    }


}
