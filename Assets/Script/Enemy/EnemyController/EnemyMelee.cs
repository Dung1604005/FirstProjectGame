
using UnityEngine;

public class EnemyMelee : EnemyBase
{

    private float lastDamageTime = 0f;

    private void AttackMelee(float x, float y)
    {
        if (cur_coolDown >= enemyBaseData.CoolDown)
        {
            attacking = true;
            
            anim.SetTrigger("IsAttack");
            anim.SetFloat(GameConfig.MOVEX_FLOAT, x);
            anim.SetFloat(GameConfig.MOVEY_FLOAT, y);
            
            
        }

    }

    public void CauseDamage()
    {
        if (Time.time - lastDamageTime < 0.1f) return;
    
        lastDamageTime = Time.time; // Cập nhật lại mốc thời gian
        Vector2 dir = (player.position - transform.position);
        float dis = dir.sqrMagnitude;

        if (dis <= enemyBaseData.RangeAtk * enemyBaseData.RangeAtk)
        {
            
            player.GetComponent<Health>().OnDamaged(attack);
            
        }
    }

    // Ket thuc tan cong va Gay damage
    public void EndAttack()
    {
        attacking = false;

        cur_coolDown = 0f;
    }
    // Quan li Trang thai tan cong
    protected override void OnAttack()
    {
        if (attacking) return;
        Vector2 dir = player.position - transform.position;
        float dis = dir.sqrMagnitude;
        AnimMove(animTypeAttack, dir.x, dir.y);
        float disPlayerFromSpawn = ((Vector2)player.position - spawnPosition).sqrMagnitude;
        float disEnemyFromSpawn = ((Vector2)transform.position - spawnPosition).sqrMagnitude;
        if (dis > enemyBaseData.RangeAtk * enemyBaseData.RangeAtk)
        {
            if (dis <= rangeChase * rangeChase && disPlayerFromSpawn <= maxMoveRadius*maxMoveRadius && disEnemyFromSpawn <= maxMoveRadius*maxMoveRadius)
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
