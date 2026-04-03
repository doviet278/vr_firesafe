using UnityEngine;

// Kế thừa IInteractable (Đã có sẵn trong Core của bạn)
public class HazardItem : MonoBehaviour, IInteractable
{
    [Header("Hazard Info")]
    public string hazardName = "O dien bi ho";

    public string GetInteractPrompt()
    {
        return "Nhan F de danh dau nguy co: " + hazardName;
    }

    public void OnInteract()
    {
        // Báo cáo về máy chủ
        GameManager.Instance.ReportHazardFound();
        
        // Ẩn vật thể này đi (hoặc đổi vật liệu thành màu xanh lá để biểu thị đã xử lý)
        gameObject.SetActive(false); 
    }
}