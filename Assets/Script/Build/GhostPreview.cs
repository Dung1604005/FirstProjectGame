using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPreview : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private bool canPlace = true;
    public bool CanPlace => canPlace;

    private int curCollision = 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    void Start()
    {
        SetCanPlace();

    }

    public void SetSprite(Sprite _sprite)
    {
        spriteRenderer.sprite = _sprite;
        
    }

    public void SetWarning()
    {
        canPlace = false;
        Color _color;
        if (ColorUtility.TryParseHtmlString("#E5393566", out _color))
        {
            spriteRenderer.color = _color;
        }

    }
    public void SetCanPlace()
    {
        canPlace = true;
        Color _color;
        if (ColorUtility.TryParseHtmlString("#4CAF5066", out _color))
        {
            spriteRenderer.color = _color;
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        curCollision += 1;


        SetWarning();

    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        curCollision -= 1;
        if (curCollision == 0)
        {
            SetCanPlace();
        }
      
    }

    public void SetPos(float x, float y)
    {
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(x, y, 0), 1); 
    }
}
