using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialGun: Gun
{


    Vector2[] arrayDir = {new Vector2(0, -1),new Vector2(-1, 0), new Vector2(1, 0),  new Vector2(0, 1)};
   
    
    public override void UpdateAnim(float dirX, float dirY)
    {
        
        Vector2 a = new Vector2(dirX, dirY).normalized;

        float speed = a.sqrMagnitude;
        if (!(speed > 0.00001))
        {
            return;
        }
        DirType dirType = GameManageMent.Instance.CalculateDirType(dirX, dirY);
        if (dirType == DirType.DOWN)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, -90);
                spriteRenderer.sortingOrder = 1;
                this.spriteRenderer.flipX = false;
        }
        else if(dirType == DirType.LEFT)
        {
            this.transform.rotation = Quaternion.Euler(0, 0,0);
            this.spriteRenderer.flipX = true;
                spriteRenderer.sortingOrder = 1;
        }
        else if(dirType == DirType.RIGHT)
        {
            this.transform.rotation = Quaternion.Euler(0, 0,0);
            this.spriteRenderer.flipX = false;
                spriteRenderer.sortingOrder = 1;
        }
        else if(dirType == DirType.UP)
        {
            this.spriteRenderer.flipX = false;
            this.transform.rotation = Quaternion.Euler(0, 0, 90);
                spriteRenderer.sortingOrder = 0;

        }
        
        // if(Math.Abs(dirY) > 0.01f)
        // {
        //     this.spriteRenderer.flipX = false;
        //     if(dirY < 0)
        //     {
        //         this.transform.rotation = Quaternion.Euler(0, 0, -90);
        //         spriteRenderer.sortingOrder = 1;
        //     }
        //     else
        //     {
        //         this.transform.rotation = Quaternion.Euler(0, 0, 90);
        //         spriteRenderer.sortingOrder = 0;
                
        //     }
        // }
        // else
        // {
        //     this.transform.rotation = Quaternion.Euler(0, 0,0);
        //     if(dirX >= 0)
        //     {

        //         this.spriteRenderer.flipX = false;
        //         spriteRenderer.sortingOrder = 1;
        //     }
        //     else
        //     {
        //         this.spriteRenderer.flipX = true;
        //         spriteRenderer.sortingOrder = 1;
        //     }
        // }
       
    }
    

    
}
