using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class AutoScrollParallax : MonoBehaviour
{
    [Header("Cài đặt Tốc độ")]
    [SerializeField] float scrollSpeed = 0.1f;

    private RawImage rawImage;
    private float currentX;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        // Tịnh tiến vị trí trục X theo tốc độ và thời gian
        currentX += scrollSpeed * Time.deltaTime;

        // Reset về 0 khi vượt quá 1 để biến số không bị quá to gây lag bộ nhớ
        if (currentX > 1f) 
        {
            currentX -= 1f;
        }

        // Cập nhật khung nhìn (UV Rect) để tạo cảm giác hình đang trôi
        // Tham số: x, y, width, height (y = 0 vì ta không trôi dọc)
        rawImage.uvRect = new Rect(currentX, 0f, 1f, 1f);
    }
}