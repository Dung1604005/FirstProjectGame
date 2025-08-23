using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pistol_Bullet : MonoBehaviour
{
    [SerializeField] private  float radius_gun = 1.5f;
    [SerializeField] private float  radius_bullet = 2f;

    [SerializeField] private float moveSpeed_gun = 0;
    [SerializeField] private GunData weaponData;
    [SerializeField] private PlayerController player;

    



    void Awake()
    {

    }
    // Ban theo huong con chuot
    public void FirePistol()
    {

        Vector2 playerPos = player.getPos();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = (mousePos - playerPos).normalized;
        Vector2 reach = playerPos + dir * radius_bullet;
        GameObject bullet = Instantiate(weaponData.Bullet, reach, Quaternion.identity);       
        dir = (mousePos - (Vector2)bullet.transform.position).normalized;
        bullet.GetComponent<BulletController>().SetDamaged(weaponData.Damaged);
        bullet.GetComponent<BulletController>().Fire(dir);
    }

    void Start()
    {

    }

    // Xoay sung theo con chuot
    void Rotate()
    {
        Vector2 playerPos = player.getPos(); 
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = (mousePos - playerPos).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position = Vector2.Lerp(transform.position, playerPos + dir * radius_gun, moveSpeed_gun*Time.fixedDeltaTime) ;
    }

    void Update()
    {

    }
    void LateUpdate()
    {
        Rotate();
    }
}
