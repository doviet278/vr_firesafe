using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Cấu trúc Singleton để các script khác có thể gọi GameManager.Instance từ bất cứ đâu
    public static GameManager Instance { get; private set; }

    [Header("Level Settings")]
    [Tooltip("Số lượng nguy cơ cần tìm. Đặt = 0 nếu đây là Scene Sandbox chỉ cần bấm nút.")]
    public int totalHazardsToFind = 0; 
    private int hazardsFound = 0;

    [Header("Victory UI Components")]
    public GameObject timeupPopup; // Khung giao diện chúc mừng
    public TMP_Text tipText; // Dòng chữ hiển thị kiến thức

    [Header("Knowledge Tips")]
    [TextArea(2, 5)]
    public string[] fireSafetyTips; // Danh sách các mẹo/kiến thức PCCC sẽ hiện ngẫu nhiên

    public float timeLimit = 120f; // thời gian (giây), ví dụ 2 phút
    private float currentTime;

    public AudioSource countdownAudio; // THÊM MỚI: Kéo AudioSource chứa file 10s vào đây
    private bool isCountdownStarted = false;

    public TMP_Text timerText; // TextMeshPro hiển thị mm:ss
    public bool isShowDialog = true;
    void Awake()
    {
        // Khởi tạo Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerUI();
    }

    void Update()
    {
        if (isShowDialog)
        {
            return;
        }

        if (Time.timeScale == 0f) return; // nếu game đã dừng (win) thì không chạy nữa

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            // Bắt đầu đếm ngược âm thanh khi còn 10 giây
            if (currentTime <= 11f && !isCountdownStarted)
            {
                if (countdownAudio != null)
                {
                    countdownAudio.Play();
                }
                isCountdownStarted = true; // Đánh dấu đã phát để Update không gọi lại Play() nữa
            }

            if (currentTime < 0) currentTime = 0;

            UpdateTimerUI();
        }
        else
        {
            TimeUp();
        }
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

        if (countdownAudio != null && countdownAudio.isPlaying) countdownAudio.Stop();
        
        // Dừng toàn bộ thời gian vật lý và chuyển động trong game
        Time.timeScale = 0f; 

        // Hiển thị lại con trỏ chuột để người chơi có thể bấm nút
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Bật giao diện chiến thắng
        UIScenPratice.Instance.ShowLosePopup();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimeUp()
    {
        Debug.Log("HET GIO!");

        if (countdownAudio != null) countdownAudio.Stop();

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (timeupPopup != null)
        {
            timeupPopup.SetActive(true);
            if (tipText != null)
            {
                tipText.text = "BAN DA HET GIO!";
            }
        }
    }
}