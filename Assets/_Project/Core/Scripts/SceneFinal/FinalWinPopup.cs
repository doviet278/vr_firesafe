using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalWinPopup : MonoBehaviour
{
    [SerializeField] private GameTaskManager taskManager;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject[] activeStars = new GameObject[3];
    [SerializeField] private GameObject[] inactiveStars = new GameObject[3];

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (taskManager == null) taskManager = GameTaskManager.Instance;

        int completedCount = taskManager != null ? taskManager.GetCompletedCount() : 0;
        int totalCount = taskManager != null ? taskManager.GetTotalCount() : 0;
        int starCount = GetStarCount(completedCount);

        UpdateStars(starCount);

        if (messageText != null)
        {
            messageText.text = GetMessage(starCount, completedCount, totalCount);
        }
    }

    private int GetStarCount(int completedCount)
    {
        if (completedCount <= 0) return 0;
        if (completedCount <= 2) return 1;
        if (completedCount <= 4) return 2;
        return 3;
    }

    private void UpdateStars(int starCount)
    {
        for (int i = 0; i < 3; i++)
        {
            bool active = i < starCount;

            if (activeStars != null && i < activeStars.Length && activeStars[i] != null)
            {
                activeStars[i].SetActive(active);
            }

            if (inactiveStars != null && i < inactiveStars.Length && inactiveStars[i] != null)
            {
                inactiveStars[i].SetActive(!active);
            }
        }
    }

    private string GetMessage(int starCount, int completedCount, int totalCount)
    {
        if (totalCount <= 0) return "Bạn chưa có nhiệm vụ nào";
        if (starCount <= 0) return "Bạn chưa hoàn thành nhiệm vụ nào";
        if (starCount == 1) return "Bạn xử lý còn chưa tốt";
        if (starCount == 2) return "Bạn xử lý khá tốt";
        return "Bạn đã hoàn thành xuất sắc";
    }
}