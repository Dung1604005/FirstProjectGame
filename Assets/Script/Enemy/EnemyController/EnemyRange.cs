
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRange: EnemyBase
{
    private float dirX, dirY;
    private float radius_bullet;

    private void SetDir(float x, float y)
    {
        dirX = x;
        dirY = y;
    }

    private void SpawnBullet()
    {
       
        
        Vector2 spawnPos = (Vector2)this.gameObject.transform.position + new Vector2(dirX, dirY)*radius_bullet;
        BulletController bullet = GameManageMent.Instance.PoolManager.BulletPoolsList[((enemyBaseData  as EnemyRangeData).IndexBullet)].Spawn(spawnPos);
        bullet.SetInfo(attack, (enemyBaseData as EnemyRangeData).IndexBullet);
        bullet.Fire(new Vector2(dirX, dirY));
        
    }
    private void AttackRange(float x, float y)
    {
        if(cur_coolDown < enemyBaseData.CoolDown)
        {
            return;
        }
        
        Vector2 dir = new Vector2(x, y).normalized;
        SetDir(dir.x, dir.y);
        anim.SetTrigger("IsAttack");
        anim.SetFloat(GameConfig.MOVEX_FLOAT, dirX);
        anim.SetFloat(GameConfig.MOVEY_FLOAT, dirY);
        attacking = true;

        
    }
    public void EndAttack()
    {
        attacking = false;
        cur_coolDown = 0f;
    }
    protected override void Init()
    {
        base.Init();
        radius_bullet = (enemyBaseData as EnemyRangeData).RadiusBullet;
    }
    protected override void OnAttack()
    {
        Vector2 dir = player.position - transform.position;
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
            if (attacking)
            {
                return;
            }

            AttackRange(dir.x, dir.y);

        }
    }
}
