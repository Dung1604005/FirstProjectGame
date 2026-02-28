using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioEnforcer : MonoBehaviour
{
    [Header("Tỷ lệ khung hình mong muốn (Mặc định 16:9)")]
    public float targetAspect = 1920f / 1080f;

    void Start()
    {
        EnforceAspectRatio();
    }

    // Nếu game của bạn cho phép người chơi kéo giãn cửa sổ khi đang chơi, 
    // hãy đổi Start() thành Update(). Nhưng thường Start() là đủ.
    public void EnforceAspectRatio()
    {
        // Xác định tỷ lệ màn hình thực tế của người chơi
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // Tính toán độ lệch tỷ lệ
        float scaleHeight = windowAspect / targetAspect;

        Camera cam = GetComponent<Camera>();

        // Nếu màn hình người chơi vuông hơn 16:9 (Ví dụ: 4:3) 
        // -> Thêm viền đen ở TRÊN và DƯỚI (Letterbox)
        if (scaleHeight < 1.0f)
        {
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            cam.rect = rect;
        }
        // Nếu màn hình người chơi dài hơn 16:9 (Ví dụ: Màn Ultra-wide 21:9)
        // -> Thêm viền đen ở TRÁI và PHẢI (Pillarbox)
        else 
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    }
}