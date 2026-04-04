using UnityEngine;

public class FinishButton : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt()
    {
        return "Nhan F de hoan thanh khoa huan luyen";
    }

    public void OnInteract()
    {
        // Bỏ qua đếm số lượng, ép buộc kích hoạt chiến thắng ngay lập tức
        GameManager.Instance.TriggerVictory();
    }
}