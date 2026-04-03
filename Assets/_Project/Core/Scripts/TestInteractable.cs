using UnityEngine;

// Lưu ý: Phải thêm ", IInteractable" bên cạnh MonoBehaviour
public class TestInteractable : MonoBehaviour, IInteractable 
{
    // 1. Bắt buộc phải có hàm này theo luật của IInteractable
    public string GetInteractPrompt()
    {
        return "Nhan F de tuong tac voi Khoi Hop";
    }

    // 2. Bắt buộc phải có hàm này, đây là nơi chứa logic khi nhấn phím
    public void OnInteract()
    {
        // Báo cáo ra màn hình Console
        Debug.Log("Thanh cong! Ban vua tuong tac voi: " + gameObject.name);

        // Đổi màu khối hộp thành màu Xanh lá để test trực quan
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.green;
        }
    }
}