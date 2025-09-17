using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSorting : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    private Tilemap tilemap;

    void Start()
    {
        // Lấy TilemapRenderer và Tilemap của đối tượng này
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemap = GetComponent<Tilemap>();
    }

    void Update()
    {
        // Cập nhật sortingOrder dựa trên vị trí Y của Tilemap (hoặc bạn có thể dùng Y của camera nếu cần)
        tilemapRenderer.sortingOrder = Mathf.FloorToInt(transform.position.y * 100);
    }
}
