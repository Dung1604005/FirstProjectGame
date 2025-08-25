using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private float radius_bullet;
    [SerializeField] private float delayTimeAfterShoot;
    [SerializeField] private float angleShotGun;
    [SerializeField] private float strengthShake;

    private CinemachineImpulseSource cinemachineImpulseSource;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        attacking = false;
    }
    public override void UpdateAnim(float dirX, float dirY)
    {
        
        if (dirX == 0f && dirY == 0f)
        {
            // ANimation down
            anim.SetFloat("DirX", 0);
            anim.SetFloat("DirY", -1);

        }
        else
        {
            anim.SetFloat("DirX", dirX);
            anim.SetFloat("DirY", dirY);
        }

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
        if (weaponData.ItemName == "ShotGun")
        {
            // Ban 2 vien lech goc angleShotGun
            Debug.Log("SHOTGUN");
            GameObject bullet2 = Instantiate(gunData.Bullet, reach, Quaternion.identity);
            GameObject bullet3 = Instantiate(gunData.Bullet, reach, Quaternion.identity);
            bullet2.GetComponent<BulletController>().SetDamaged(weaponData.Damaged);
            

            bullet2.GetComponent<BulletController>().Fire(Quaternion.Euler(0, 0, angleShotGun)*dir);
            bullet3.GetComponent<BulletController>().SetDamaged(weaponData.Damaged);
            bullet3.GetComponent<BulletController>().Fire(Quaternion.Euler(0, 0,-angleShotGun)*dir);


        }
        bullet.GetComponent<BulletController>().SetDamaged(weaponData.Damaged);
        bullet.GetComponent<BulletController>().Fire(dir);
        // Them vao sau hieu ung shake
        if (weaponData.ItemName == "ShotGun")
        {
            Debug.Log("SHOOT");
            cinemachineImpulseSource.GenerateImpulse(strengthShake);
        }
        
        

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
