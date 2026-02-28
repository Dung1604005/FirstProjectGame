using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; // BẮT BUỘC THÊM DÒNG NÀY ĐỂ GỌI TEXTMESHPRO

public class HoverButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Thành phần UI")]
    public RectTransform textToMove; 
    public GameObject arrowIcon;

    public GameObject BgImage;

    [Header("Cài đặt Chữ (Text)")]
    public float textPushDistance = 15f;
    public float textSmoothSpeed = 12f;
    
    [Tooltip("Kéo thả component TextMeshPro của chữ vào đây")]
    public TextMeshProUGUI buttonText; // Biến mới để điều khiển màu chữ
    public Color defaultColor = Color.white; // Màu bình thường
    public Color hoverColor = Color.yellow;  // Màu khi chuột trỏ vào

    [Header("Cài đặt Mũi tên (Arrow)")]
    public float arrowMoveDistance = 5f; 
    public float arrowMoveSpeed = 15f;   

    private Vector2 textOriginalPos;
    private Vector2 textTargetPos;

    private RectTransform arrowRect;
    private Vector2 arrowBasePos;
    private bool isHovering = false; 

    private void Start()
    {
        // Khởi tạo vị trí gốc của chữ
        if (textToMove != null)
        {
            textOriginalPos = textToMove.anchoredPosition;
            textTargetPos = textOriginalPos;
        }

        // Khởi tạo vị trí gốc của mũi tên
        if (arrowIcon != null)
        {
            arrowRect = arrowIcon.GetComponent<RectTransform>();
            if (arrowRect != null)
            {
                arrowBasePos = arrowRect.anchoredPosition;
            }
            arrowIcon.SetActive(false);
        }

        // Set màu mặc định ban đầu
        if (buttonText != null)
        {
            buttonText.color = defaultColor;
        }
    }

    private void Update()
    {
        // 1. Lướt chữ mượt mà
        if (textToMove != null)
        {
            textToMove.anchoredPosition = Vector2.Lerp(textToMove.anchoredPosition, textTargetPos, Time.deltaTime * textSmoothSpeed);
        }

        // 2. Hiệu ứng Mũi tên nhấp nhô
        if (isHovering && arrowRect != null)
        {
            float offsetX = Mathf.Sin(Time.time * arrowMoveSpeed) * arrowMoveDistance;
            arrowRect.anchoredPosition = new Vector2(arrowBasePos.x + offsetX, arrowBasePos.y);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BgImage.SetActive(true);
        isHovering = true;
        if (arrowIcon != null) arrowIcon.SetActive(true);
        
        // Đẩy chữ sang phải và ĐỔI MÀU HOVER
        textTargetPos = textOriginalPos + new Vector2(textPushDistance, 0f); 
        if (buttonText != null) buttonText.color = hoverColor;
    }
    

    public void OnPointerExit(PointerEventData eventData)
    {
        BgImage.SetActive(false);
        isHovering = false;
        if (arrowIcon != null)
        {
            arrowIcon.SetActive(false);
            if (arrowRect != null) arrowRect.anchoredPosition = arrowBasePos;
        }
        
        // Kéo chữ lùi lại và TRẢ VỀ MÀU MẶC ĐỊNH
        textTargetPos = textOriginalPos; 
        if (buttonText != null) buttonText.color = defaultColor;
    }
}