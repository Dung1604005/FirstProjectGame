using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Schema;
using Cinemachine;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private float radius_bullet;
    [SerializeField] private float delayTimeAfterShoot;
    [SerializeField] private float angleShotGun;
    [SerializeField] private float strengthShake;

    [SerializeField] private int curBullet;
    public int CurBullet => curBullet;

    private int totalBullet;
    public int TotalBullet => totalBullet;

    private bool reloading = false;
    public bool Reloading => reloading;

    private float timeReload = 0f;


    private CinemachineImpulseSource cinemachineImpulseSource;
    void UpdateCurStateBullet()
    {
        if (curBullet / (float)(WeaponData as GunData).MagSize >= 0.5f)
        {
            UIManageMent.Instance.BulletUIController.SetStateCurrentBulletColor(GameConfig.COLORWHITERELOAD);
        }
        else if (curBullet / (float)(WeaponData as GunData).MagSize >= 0.2f)
        {
            UIManageMent.Instance.BulletUIController.SetStateCurrentBulletColor(GameConfig.COLORYELLOWRELOAD);
        }
        else
        {
            UIManageMent.Instance.BulletUIController.SetStateCurrentBulletColor(GameConfig.COLORREDRELOAD);
        }
    }
    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        attacking = false;
        if (weaponData != null)
        {
            if (weaponData.ItemName == "ShotGun")
            {

                totalBullet = GameManageMent.Instance.PlayerManager.ShotgunBullet;
                curBullet = GameManageMent.Instance.PlayerManager.Cur_ShotgunBullet;
            }
            else if (weaponData.ItemName == "Pistol")
            {

                totalBullet = GameManageMent.Instance.PlayerManager.PistolBullet;
                curBullet = GameManageMent.Instance.PlayerManager.Cur_PistolBullet;
            }
            else if (weaponData.ItemName == "Gun")
            {

                totalBullet = GameManageMent.Instance.PlayerManager.GunBullet;
                curBullet = GameManageMent.Instance.PlayerManager.Cur_GunBullet;
            }
        }
        
        UIManageMent.Instance.BulletUIController.UpdateBulletUI((weaponData as GunData).BulletUI, curBullet, (weaponData as GunData).MagSize, totalBullet);
        UIManageMent.Instance.BulletUIController.TurnOnBulletUI();
        
        UpdateCurStateBullet();

    }
    public override void UpdateAnim(float dirX, float dirY)
    {
        if (dirX == 0f && dirY == 0f)
        {
            // ANimation down
            dirY = -1f;

        }
        
        
        
        if (dirX > 0.01f || dirX < -0.01f)
        {
            spriteRenderer.sortingOrder = 2;
        }
        else if (dirY < -0.01f)
        {
            spriteRenderer.sortingOrder = 2;
        }
        else
        {
            spriteRenderer.sortingOrder = 0;
        }

        
        anim.SetFloat("DirX", dirX);
        anim.SetFloat("DirY", dirY);
        

    }
    IEnumerator Couroutine(float time)
    {

        yield return new WaitForSeconds(time);

        EndAttack();

    }
    IEnumerator WaitTimeReload(float time)
    {
        reloading = true;
        timeReload = 0f;
        UIManageMent.Instance.TurnOnReloadingText();    
        yield return new WaitForSeconds(time);
        reloading = false;
        if (totalBullet >= (weaponData as GunData).MagSize - curBullet)
        {
            

            totalBullet -= (weaponData as GunData).MagSize - curBullet;

            curBullet = (weaponData as GunData).MagSize;
            GameManageMent.Instance.PlayerManager.UpdateTotalBullet(weaponData.ItemName, totalBullet);
            GameManageMent.Instance.PlayerManager.UpdateCurrentBullet(weaponData.ItemName, curBullet);

        }
        else
        {
            
            
            curBullet += totalBullet;

            totalBullet = 0;
            GameManageMent.Instance.PlayerManager.UpdateTotalBullet(weaponData.ItemName, totalBullet);
            GameManageMent.Instance.PlayerManager.UpdateCurrentBullet(weaponData.ItemName, curBullet);
        }
        UIManageMent.Instance.TurnOffReloadingText();
        UIManageMent.Instance.BulletUIController.UpdateBulletUI((weaponData as GunData).BulletUI, curBullet, (weaponData as GunData).MagSize, totalBullet);
        UpdateCurStateBullet();
    }

    public void Fire()
    {
        GunData gunData = weaponData as GunData;
        Vector2 playerPos = GameManageMent.Instance.PlayerManager.PlayerController.getPos();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = (mousePos - playerPos).normalized;
        Vector2 reach = playerPos + dir * radius_bullet;
        GameObject bullet = Instantiate(gunData.Bullet, reach, Quaternion.identity);


        
        if (weaponData.ItemName == "ShotGun")
        {
            // Ban 2 vien lech goc angleShotGun

            GameObject bullet2 = Instantiate(gunData.Bullet, reach, Quaternion.identity);
            GameObject bullet3 = Instantiate(gunData.Bullet, reach, Quaternion.identity);
            bullet2.GetComponent<BulletController>().SetDamaged(weaponData.Damaged);


            bullet2.GetComponent<BulletController>().Fire(Quaternion.Euler(0, 0, angleShotGun) * dir);
            bullet3.GetComponent<BulletController>().SetDamaged(weaponData.Damaged);
            bullet3.GetComponent<BulletController>().Fire(Quaternion.Euler(0, 0, -angleShotGun) * dir);


        }
        bullet.GetComponent<BulletController>().SetDamaged(weaponData.Damaged);
        bullet.GetComponent<BulletController>().Fire(dir);
        // Them vao sau hieu ung shake
        if (weaponData.ItemName == "ShotGun")
        {
            
            cinemachineImpulseSource.GenerateImpulse(strengthShake);
        }
        
        curBullet -= 1;
        GameManageMent.Instance.PlayerManager.UpdateCurrentBullet(weaponData.ItemName, curBullet);
        UpdateCurStateBullet();
        UIManageMent.Instance.BulletUIController.UpdateCurrentBullet(curBullet);

        StartCoroutine(Couroutine(delayTimeAfterShoot));

    }
    public override void Attack(float dirX, float dirY)
    {
        if(reloading)
        {
            UIManageMent.Instance.UpdateWarning("Reloading...");
            UIManageMent.Instance.TurnOnWarning();
            return;
        }
        if(curBullet <= 0)
        {
            UIManageMent.Instance.UpdateWarning("Out of Bullet, Reload!");
            UIManageMent.Instance.TurnOnWarning();
            return;
        }
        float angle = Mathf.Atan2(dirY, dirX);
        float y = Mathf.Sin(angle);
        float x = Mathf.Cos(angle);
        attacking = true;
        GameManageMent.Instance.PlayerManager.PlayerController.AnimUpdate(x, y);
        UpdateAnim(x, y);
        Fire();


    }
    public void Reload()
    {
        if (!reloading)
        {

            if (totalBullet > 0)
            {
                StartCoroutine(WaitTimeReload((weaponData as GunData).ReloadTime));
            }
            else
            {
                UIManageMent.Instance.UpdateWarning("No Bullet Left!");
                UIManageMent.Instance.TurnOnWarning();
            }

        }
        else
        {
            UIManageMent.Instance.UpdateWarning("Reloading...");
            UIManageMent.Instance.TurnOnWarning();
        }
    }

    void Update()
    {
        if (!reloading)
        {
            
            if (timeReload < (weaponData as GunData).ReloadTime)
            {
                timeReload += Time.deltaTime;
            }
        }
    }

}
