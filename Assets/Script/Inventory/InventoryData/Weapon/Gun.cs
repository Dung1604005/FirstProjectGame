using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private float radius_bullet;
    [SerializeField] private float delayTimeAfterShoot;
    void Awake()
    {
        anim = GetComponent<Animator>();
        attacking = false;
    }
    public override void UpdateAnim(float dirX, float dirY)
    {
        
        anim.SetFloat("DirX", dirX);
        anim.SetFloat("DirY", dirY);
        
    }
    IEnumerator Couroutine(float time)
    {
        
        yield return new WaitForSeconds(time);
        
        EndAttack();
    
    }
    public void Fire()
    {
        GunData gunData = weaponData as GunData;
        Vector2 playerPos = PlayerController.Instance.getPos();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = (mousePos - playerPos).normalized;
        Vector2 reach = playerPos + dir * radius_bullet;
        GameObject bullet = Instantiate(gunData.Bullet, reach, Quaternion.identity);
        dir = (mousePos - (Vector2)bullet.transform.position).normalized;
        bullet.GetComponent<BulletController>().SetDamaged(weaponData.Damaged);
        bullet.GetComponent<BulletController>().Fire(dir);
        
        StartCoroutine(Couroutine(delayTimeAfterShoot));

    }
    public override void Attack(float dirX, float dirY)
    {

        float angle = Mathf.Atan2(dirY, dirX);
        float y = Mathf.Sin(angle);
        float x = Mathf.Cos(angle);
        attacking = true;
        PlayerController.Instance.AnimUpdate(x, y);
        UpdateAnim(x, y);
        Fire();
        

    }
    
}
