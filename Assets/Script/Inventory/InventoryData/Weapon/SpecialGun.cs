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
        Debug.Log(dirX + " " + dirY);
        Vector2 a = new Vector2(dirX, dirY).normalized;
        Vector2 left = new Vector2(-1, 0);
        Vector2 right = new Vector2(1, 0);
        Vector2 up = new Vector2(0, 1);
        Vector2 down = new Vector2(0, -1);
        float speed = a.sqrMagnitude;
        if (!(speed > 0.00001))
        {
            return;
        }
        int distanceDir = 10;
        Vector2 animDir= Vector2.zero;
        for(int i = 0; i < arrayDir.Length; i++)
        {
            if(distanceDir > (int)(arrayDir[i] - new Vector2(dirX, dirY)).sqrMagnitude)
            {
                distanceDir = Math.Min(distanceDir, (int)(arrayDir[i] - new Vector2(dirX, dirY)).sqrMagnitude);
                animDir = arrayDir[i];
            }
    
        }
        if (animDir == down)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, -90);
                spriteRenderer.sortingOrder = 1;
                this.spriteRenderer.flipX = false;
        }
        else if(animDir == left)
        {
            this.transform.rotation = Quaternion.Euler(0, 0,0);
            this.spriteRenderer.flipX = true;
                spriteRenderer.sortingOrder = 1;
        }
        else if(animDir == right)
        {
            this.transform.rotation = Quaternion.Euler(0, 0,0);
            this.spriteRenderer.flipX = false;
                spriteRenderer.sortingOrder = 1;
        }
        else if(animDir == up)
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
