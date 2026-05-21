using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskEvaluationUI : MonoBehaviour
{
    public GameTaskManager taskManager;

    [Header("UI References")]
    public GameObject evaluationPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI percentText;
    public TextMeshProUGUI messageText;
    public Slider progressBar;

    private void Reset()
    {
        taskManager = GameTaskManager.Instance;
    }

    private void OnEnable()
    {
        if (taskManager == null) taskManager = GameTaskManager.Instance;
        if (taskManager != null) taskManager.OnTaskChanged += HandleTaskChanged;
        UpdateUI(taskManager != null ? taskManager.GetCompletionPercent() : 0f);
    }

    private void OnDisable()
    {
        if (taskManager != null) taskManager.OnTaskChanged -= HandleTaskChanged;
    }

    private void HandleTaskChanged(string id, bool completed, float percent)
    {
        UpdateUI(percent);
    }

    public void ShowEvaluation()
    {
        float percent = taskManager != null ? taskManager.GetCompletionPercent() : 0f;
        UpdateUI(percent);
        if (evaluationPanel != null) evaluationPanel.SetActive(true);
    }

    private void UpdateUI(float percentFloat)
    {
        int percent = Mathf.Clamp(Mathf.RoundToInt(percentFloat), 0, 100);
        if (percentText != null) percentText.text = percent + "%";
        if (progressBar != null) progressBar.value = percent / 100f;

        if (percent == 100)
        {
            if (titleText != null) titleText.text = "Hoàn thành xuất sắc";
            if (messageText != null) messageText.text = "Bạn đã hoàn thành tất cả nhiệm vụ!";
        }
        else
        {
            if (titleText != null) titleText.text = "Đánh giá nhiệm vụ";
            if (messageText != null) messageText.text = GetEvaluationForPercent(percent);
        }
    }

    private string GetEvaluationForPercent(int p)
    {
        if (p <= 0) return $"Đạt 0% — Chưa hoàn thành nhiệm vụ nào.";
        return $"Đạt {p}% — Đánh giá số {p}.";
    }
}
