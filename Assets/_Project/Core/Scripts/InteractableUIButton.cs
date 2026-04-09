using UnityEngine;
using UnityEngine.UI; // Bắt buộc phải có thư viện này để điều khiển Button

// Kế thừa IInteractable để hệ thống Raycast của nhân vật nhận diện được
public class InteractableUIButton : MonoBehaviour, IInteractable
{
    private Button uiButton;

    void Awake()
    {
        // Lấy Component Button có sẵn trên đối tượng này
        uiButton = GetComponent<Button>();
    }

    public string GetInteractPrompt()
    {
        // Tên hiển thị khi người chơi ngắm vào nút
        // Có thể lấy luôn tên của GameObject để làm prompt cho lẹ
        return "Nhan F de chon: " + gameObject.name; 
    }

    public void OnInteract()
    {
        if (uiButton != null && uiButton.interactable)
        {
            // Lệnh quan trọng nhất: Ép nút UI chạy lệnh OnClick y hệt như khi bị click bằng chuột thật
            uiButton.onClick.Invoke(); 
        }
    }
}