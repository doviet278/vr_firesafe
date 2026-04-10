using UnityEngine;
using UnityEngine.SceneManagement; // THÊM MỚI: Khai báo thư viện để chuyển cảnh

public class SceneTrainExtinguisherManager : MonoBehaviour
{
    public static SceneTrainExtinguisherManager Instance;
    
    [Header("Level Settings")]
    public int totalFireItems = 12;
    [SerializeField] private GameObject completedPopup;
    
    [Header("Next Scene Settings")]
    [Tooltip("Gõ tên Scene bạn muốn chuyển đến khi bấm nút (VD: MainMenu)")]
    public string nextSceneName = "Scene1B"; // THÊM MỚI: Biến để điền tên màn tiếp theo

    private int extinguishedCount = 0;
    public bool isCompleted = false;

    private void Awake()
    {
        Instance = this;
    }

    // Gọi hàm này mỗi khi dập 1 item
    public void ReportFireExtinguished()
    {
        if (isCompleted) return;

        extinguishedCount++;
        Debug.Log($"Đã dập: {extinguishedCount} / {totalFireItems}"); // Chuyển LogError thành Log thường để Console đỡ bị báo đỏ

        if (extinguishedCount >= totalFireItems)
        {
            CompleteRoom();
        }
    }

    void CompleteRoom()
    {
        isCompleted = true;
        Debug.Log("HOÀN THÀNH!");
        
        if (completedPopup != null)
        {
            completedPopup.SetActive(true);
        }

        // --- BẮT ĐẦU LOGIC HIỆN CHUỘT VÀ DỪNG GAME ---
        
        // 1. Dừng toàn bộ thời gian vật lý của game
        Time.timeScale = 0f; 

        // 2. Mở khóa con trỏ chuột khỏi trung tâm màn hình
        Cursor.lockState = CursorLockMode.None;

        // 3. Hiển thị con trỏ chuột lên
        Cursor.visible = true;
        
        // ----------------------------------------------
    }

    // THÊM MỚI: Hàm này sẽ được gọi khi bạn click vào nút trên UI
    public void GoToNextScene()
    {
        // BẮT BUỘC: Phải trả lại thời gian về 1 trước khi sang scene mới, nếu không scene mới cũng bị đơ
        Time.timeScale = 1f; 
        
        // Chuyển sang màn hình đã thiết lập
        SceneManager.LoadScene(nextSceneName);
    }
}