
using UnityEngine;

public class GhostSprite : MonoBehaviour, IPoolable
{


    private SpriteRenderer spriteRenderer;
    private Color currentColor;

    [SerializeField] private float fadeSpeed;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetInfo(Sprite sprite, bool isFlipX, Color color)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.flipX = isFlipX;

        spriteRenderer.color = color;

        currentColor = color;
    }
    public void OnSpawn()
    {
        
    }

    public void OnDeSpawn()
    {
        
    }

    void DeSpawn()
    {
        GameManageMent.Instance.PoolManager.GhostSpritePools.DeSpawn(this);
    }

    void Update()
    {
        currentColor.a -= fadeSpeed*Time.deltaTime;
        spriteRenderer.color = currentColor;
        if(currentColor.a <= 0f)
        {
           DeSpawn();
        }
    }
}
