using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PC_Interactor : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float interactRange = 3f;
    public LayerMask interactableLayer;

    [Header("Crosshair UI")]
    public Image crosshairImage;
    public Sprite defaultCrosshair;   // Dấu cộng bình thường
    public Sprite hoverCrosshair;     // Bàn tay mở
    public Sprite grabCrosshair;      // Bàn tay nắm chặt

    [Header("Holding System")]
    public Transform holdPoint;       // Vị trí vật thể sẽ neo vào khi cầm
    private GameObject heldObject;    // Lưu trữ vật thể đang cầm
    private bool isHolding = false;   // Trạng thái cầm nắm

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        HandleCrosshairUI();
        HandleInteraction();
    }

    private void HandleCrosshairUI()
    {
        // Trạng thái 3: Đang cầm vật thể -> Ép hiển thị tay nắm chặt
        if (isHolding)
        {
            if (crosshairImage.sprite != grabCrosshair)
                crosshairImage.sprite = grabCrosshair;
            return; 
        }

        // Nếu không cầm gì, bắn tia kiểm tra xem có đang nhìn vào vật thể không
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            // Trạng thái 2: Nhìn trúng vật thể -> Hiển thị tay mở
            if (crosshairImage.sprite != hoverCrosshair)
                crosshairImage.sprite = hoverCrosshair;
        }
        else
        {
            // Trạng thái 1: Nhìn vào không khí -> Hiển thị dấu cộng
            if (crosshairImage.sprite != defaultCrosshair)
                crosshairImage.sprite = defaultCrosshair;
        }
    }

    private void HandleInteraction()
    {
        // Kiểm tra xem phím F có được bấm trong khung hình này không
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (isHolding)
            {
                // Đang cầm thì thả ra
                DropObject();
            }
            else
            {
                // Không cầm gì thì thử nhặt
                TryGrabObject();
            }
        }
    }

    private void TryGrabObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            heldObject = hit.collider.gameObject;
            isHolding = true;

            // Xử lý vật lý: Tắt trọng lực để vật thể không rơi
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            // Neo vật thể vào camera
            heldObject.transform.SetParent(holdPoint);
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.identity;
            
            Debug.Log("Da nhat: " + heldObject.name);
        }
    }

    private void DropObject()
    {
        if (heldObject != null)
        {
            // Tách vật thể khỏi camera
            heldObject.transform.SetParent(null);

            // Bật lại vật lý để vật thể rơi xuống sàn
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            Debug.Log("Da tha: " + heldObject.name);
            heldObject = null;
            isHolding = false;
        }
    }
}