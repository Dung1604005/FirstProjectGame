using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Schema;
using Cinemachine;
using Unity.Jobs;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.U2D;
using UnityEngine;

public class Gun : Weapon
{

    [Header("SETTING")]
    [SerializeField] float recoilAmount;
    [SerializeField] float recoverSpeed;

    private Vector2 originPos;
    [SerializeField] private float radius_bullet;
    [SerializeField] private float delayTimeAfterShoot;
    [SerializeField] private float angleShotGun;
    [SerializeField] private float strengthShake;

    

    [SerializeField] private int curBullet;
    public int CurBullet => curBullet;

    [SerializeField] private bool haveShootFire;

    [SerializeField] private List<FireShoot> fireShoots;

    protected int totalBullet;
    public int TotalBullet => totalBullet;

    protected bool reloading = false;
    public bool Reloading => reloading;

    private float timeReload = 0f;


    protected CinemachineImpulseSource cinemachineImpulseSource;

    private Transform weaponSocket;
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
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        weaponSocket = transform.parent;
        originPos = transform.parent.localPosition;
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        attacking = false;
        if (weaponData != null)
        {
            if ((weaponData as GunData).GunType == GunType.SHOTGUN)
            {

                totalBullet = GameManageMent.Instance.PlayerManager.ShotgunBullet;
                curBullet = GameManageMent.Instance.PlayerManager.Cur_ShotgunBullet;
            }
            else if ((weaponData as GunData).GunType == GunType.PISTOL)
            {

                totalBullet = GameManageMent.Instance.PlayerManager.PistolBullet;
                curBullet = GameManageMent.Instance.PlayerManager.Cur_PistolBullet;
            }
            else if ((weaponData as GunData).GunType == GunType.GUN)
            {

                totalBullet = GameManageMent.Instance.PlayerManager.GunBullet;
                curBullet = GameManageMent.Instance.PlayerManager.Cur_GunBullet;
            }
        }
        
        UIManageMent.Instance.BulletUIController.UpdateBulletUI((weaponData as GunData).BulletUI, curBullet, (weaponData as GunData).MagSize, totalBullet);
        UIManageMent.Instance.BulletUIController.TurnOnBulletUI();
        
        UpdateCurStateBullet();

    }

    public void Recoil(float dirX, float dirY)
    {
        DirType dirType = GameManageMent.Instance.CalculateDirType(dirX, dirY);
        Vector2 recoilVector = Vector2.zero;
        if(dirType == DirType.DOWN)
        {
            recoilVector = Vector2.up*recoilAmount;

        }
        else if (dirType  == DirType.LEFT)
        {
            recoilVector = Vector2.right*recoilAmount;
        }
        else if(dirType == DirType.RIGHT)
        {
            recoilVector = Vector2.left*recoilAmount;
        }
        else
        {
            recoilVector = Vector2.down*recoilAmount;
        }
        
        weaponSocket.localPosition += (Vector3)recoilVector;
        
        
       
    }
    public override void UpdateAnim(float dirX, float dirY)
    {
        if(Mathf.Abs(dirX) + Mathf.Abs(dirY) > 0)
        {
            float _dirX = dirX;
            float _dirY = dirY;
            DirType dirType = GameManageMent.Instance.CalculateDirType(dirX, dirY);
            if(dirType == DirType.DOWN)
            {
                _dirX = 0f;
                _dirY = -1f;
            }
            else if(dirType == DirType.LEFT)
            {
                _dirX = -1f;
                _dirY = 0f;
            }
            else if(dirType == DirType.RIGHT)
            {
                _dirX = 1f;
                _dirY = 0f;
            }
            else
            {
                _dirX = 0f;
                _dirY = 1f;
            }
            anim.SetFloat("DirX", _dirX);
            anim.SetFloat("DirY", _dirY);
        }
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
            GameManageMent.Instance.PlayerManager.UpdateTotalBullet((weaponData as GunData).GunType, totalBullet);
            GameManageMent.Instance.PlayerManager.UpdateCurrentBullet((weaponData as GunData).GunType, curBullet);

        }
        else
        {
            
            
            curBullet += totalBullet;

            totalBullet = 0;
            GameManageMent.Instance.PlayerManager.UpdateTotalBullet((weaponData as GunData).GunType, totalBullet);
            GameManageMent.Instance.PlayerManager.UpdateCurrentBullet((weaponData as GunData).GunType, curBullet);
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
        Vector2 reach = Vector2.zero;
        if (haveShootFire)
        {
             DirType dirType = GameManageMent.Instance.CalculateDirType(dir.x, dir.y);
             if (dirType == DirType.DOWN)
            {
                fireShoots[0].TurnOn();
                reach += (Vector2)fireShoots[0].gameObject.transform.position;
            }
            else if(dirType == DirType.LEFT)
            {
                fireShoots[1].TurnOn();
                reach += (Vector2)fireShoots[1].gameObject.transform.position;
            }
            else if(dirType == DirType.RIGHT)
            {
                fireShoots[2].TurnOn();
                reach += (Vector2)fireShoots[2].gameObject.transform.position;
            }
            else
            {
                fireShoots[3].TurnOn();
                reach += (Vector2)fireShoots[3].gameObject.transform.position;
            }
        
        }

        else
        {
            reach += (Vector2)this.gameObject.transform.position + dir * radius_bullet;
        }
        BulletController bullet = GameManageMent.Instance.PoolManager.BulletPoolsList[(weaponData as GunData).IndexBullet].Spawn(reach);

        Recoil(dir.x, dir.y);


       
        if (weaponData.ItemName == "ShotGun")
        {
            // Ban 2 vien lech goc angleShotGun

            BulletController bullet2 = GameManageMent.Instance.PoolManager.BulletPoolsList[(weaponData as GunData).IndexBullet].Spawn(reach);
            BulletController bullet3 = GameManageMent.Instance.PoolManager.BulletPoolsList[(weaponData as GunData).IndexBullet].Spawn(reach);
            bullet2.SetInfo(weaponData.Damaged, (weaponData as GunData).IndexBullet);


            bullet2.Fire(Quaternion.Euler(0, 0, angleShotGun) * dir);
            bullet3.SetInfo(weaponData.Damaged, (weaponData as GunData).IndexBullet);
            bullet3.Fire(Quaternion.Euler(0, 0, -angleShotGun) * dir);


        }
        bullet.GetComponent<BulletController>().SetInfo(weaponData.Damaged, (weaponData as GunData).IndexBullet);
        bullet.GetComponent<BulletController>().Fire(dir);
        // Them vao sau hieu ung shake
        if (weaponData.ItemName == "ShotGun")
        {
            
            cinemachineImpulseSource.GenerateImpulse(strengthShake);
        }
        
        curBullet -= 1;
        GameManageMent.Instance.PlayerManager.UpdateCurrentBullet((weaponData as GunData).GunType, curBullet);
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
        GameManageMent.Instance.PlayerManager.PlayerController.UpdatePlayerDir(x, y);
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
        
        weaponSocket.localPosition = Vector3.Lerp(weaponSocket.localPosition, originPos, Time.deltaTime * recoverSpeed);
            
            
        

        if (!reloading)
        {
            
            if (timeReload < (weaponData as GunData).ReloadTime)
            {
                timeReload += Time.deltaTime;
            }
        }
    }

}
