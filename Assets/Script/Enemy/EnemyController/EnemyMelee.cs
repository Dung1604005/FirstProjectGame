using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMelee : EnemyBase
{

    private void AttackMelee(float x, float y)
    {
        if (cur_coolDown >= enemyBaseData.CoolDown)
        {
            attacking = true;
            cur_coolDown = 0f;
            anim.SetTrigger("IsAttack");
            anim.SetFloat("MoveX", x);
            anim.SetFloat("MoveY", y);
            
            
        }

    }

    // Ket thuc tan cong va Gay damage
    public void EndAttack()
    {
        Vector2 dir = (player.position - transform.position);
        float dis = dir.sqrMagnitude;

        if (dis <= enemyBaseData.RangeAtk * enemyBaseData.RangeAtk)
        {
            if (attacking)
            {
                player.GetComponent<Health>().OnDamaged(enemyBaseData.Atk);
            }

            
        }
        attacking = false;
    }
    // Quan li Trang thai tan cong
    protected override void OnAttack()
    {
        Vector2 dir = (player.position - transform.position);
        float dis = dir.sqrMagnitude;
        AnimMove(animTypeAttack, dir.x, dir.y);
        if (dis > enemyBaseData.RangeAtk * enemyBaseData.RangeAtk)
        {
            if (dis <= enemyBaseData.RangeChase * enemyBaseData.RangeChase)
            {

                curState = State.Chase;
            }
            else
            {
                curState = State.Idle;
            }
        }
        else
        {

            AttackMelee(dir.x, dir.y);

        }

    }
   

    
   
}
