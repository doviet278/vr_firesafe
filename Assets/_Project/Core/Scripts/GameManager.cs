using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Cấu trúc Singleton để các script khác có thể gọi GameManager.Instance từ bất cứ đâu
    public static GameManager Instance { get; private set; }

    [Header("Level Settings")]
    [Tooltip("Số lượng nguy cơ cần tìm. Đặt = 0 nếu đây là Scene Sandbox chỉ cần bấm nút.")]
    public int totalHazardsToFind = 0; 
    private int hazardsFound = 0;

    [Header("Victory UI Components")]
    public GameObject victoryPanel; // Khung giao diện chúc mừng
    public Text tipText; // Dòng chữ hiển thị kiến thức

    [Header("Knowledge Tips")]
    [TextArea(2, 5)]
    public string[] fireSafetyTips; // Danh sách các mẹo/kiến thức PCCC sẽ hiện ngẫu nhiên

    void Awake()
    {
        // Khởi tạo Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // SCENE 1: Hàm được gọi khi người chơi nhặt/nhấn vào một điểm nguy cơ
    public void ReportHazardFound()
    {
        hazardsFound++;
        Debug.Log($"Da tim thay: {hazardsFound} / {totalHazardsToFind} nguy co.");

        if (totalHazardsToFind > 0 && hazardsFound >= totalHazardsToFind)
        {
            TriggerVictory();
        }
    }

    // SCENE 2 HOẶC CÁC ĐIỀU KIỆN KHÁC: Gọi thẳng hàm này để thắng luôn
    public void TriggerVictory()
    {
        Debug.Log("CHIEN THANG!");
        
        // Dừng toàn bộ thời gian vật lý và chuyển động trong game
        Time.timeScale = 0f; 

        // Hiển thị lại con trỏ chuột để người chơi có thể bấm nút
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Bật giao diện chiến thắng
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            
            // Random kiến thức PCCC
            if (fireSafetyTips.Length > 0 && tipText != null)
            {
                int randomIndex = Random.Range(0, fireSafetyTips.Length);
                tipText.text = "KIEN THUC PCCC:\n" + fireSafetyTips[randomIndex];
            }
        }
    }

    // Hàm gắn vào nút "Về Menu" trên giao diện UI
    public void ReturnToMainMenu()
    {
        // Bắt buộc phải khôi phục thời gian về 1 trước khi chuyển cảnh
        Time.timeScale = 1f; 
        
        // Thay "MainMenu" bằng đúng tên Scene giao diện chính của bạn
        SceneManager.LoadScene("MainMenu"); 
    }
}