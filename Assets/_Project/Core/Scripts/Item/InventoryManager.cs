using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    // Singleton pattern
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Settings")]
    public int maxSlots = 5;
    public Transform holdPoint; // Vị trí hiển thị đồ trên tay Camera
    
    private List<ItemBase> inventory = new List<ItemBase>();
    private int selectedIndex = -1;
    private PlayerStats playerStats;
    private UITutorial uiTutorial;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        GameObject obj = GameObject.Find("UITutorial");
        if(obj != null)
        {
            uiTutorial = obj.GetComponent<UITutorial>();
        }
    }

    void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        HandleScrollSelection();
        HandleItemUse();
        HandleItemDrop();
    }

    public void PickupItem(ItemBase item)
    {
        if (inventory.Count >= maxSlots) return;
        if (inventory.Contains(item)) return; 

        if (playerStats.currentWeight + item.itemWeight > playerStats.maxWeightCapacity) return;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
        
        item.transform.SetParent(holdPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        inventory.Add(item);
        playerStats.currentWeight += item.itemWeight; 

        // LOGIC MỚI: Quản lý trực tiếp thay vì phụ thuộc biến âm
        if (inventory.Count == 1) 
        {
            // Nếu đây là đồ duy nhất trong túi, ÉP BUỘC trang bị nó ngay lập tức
            selectedIndex = 0;
            EquipCurrentItem();
        }
        else 
        {
            // Nếu đã có đồ trên tay, món mới nhặt mặc định bị cất đi
            item.OnUnequip();
        }
    }
    private void HandleScrollSelection()
    {
        if (inventory.Count == 0) return;

        float scrollValue = Mouse.current.scroll.ReadValue().y;
        if (scrollValue == 0f) return;

        // Cất vật phẩm hiện tại
        if (selectedIndex >= 0 && selectedIndex < inventory.Count)
        {
            inventory[selectedIndex].OnUnequip();
        }

        // Tính toán index mới
        if (scrollValue > 0f) selectedIndex--;
        else if (scrollValue < 0f) selectedIndex++;

        // Vòng lặp index
        if (selectedIndex >= inventory.Count) selectedIndex = 0;
        else if (selectedIndex < 0) selectedIndex = inventory.Count - 1;

        EquipCurrentItem();
    }

    private void EquipCurrentItem()
    {
        if (selectedIndex >= 0 && selectedIndex < inventory.Count)
        {
            inventory[selectedIndex].OnEquip();
        }
    }

    private void HandleItemUse()
    {
        if (Mouse.current.rightButton.isPressed && selectedIndex >= 0 && selectedIndex < inventory.Count)
        {
            inventory[selectedIndex].UseItem();
        }
    }

    private void HandleItemDrop()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame && selectedIndex >= 0 && selectedIndex < inventory.Count)
        {
            uiTutorial?.HideTutorialQ();
            if(TowelUIManager.Instance != null)
            {
                TowelUIManager.Instance.ShowClothOverlay(false);
            }
            ItemBase itemToDrop = inventory[selectedIndex];
            
            inventory.RemoveAt(selectedIndex);
            playerStats.currentWeight -= itemToDrop.itemWeight;

            itemToDrop.transform.SetParent(null);
            
            // Ép bật hiển thị và trả lại Layer Default cho TẤT CẢ bộ phận
            itemToDrop.gameObject.SetActive(true); 
            itemToDrop.ChangeLayerRecursively(itemToDrop.gameObject, LayerMask.NameToLayer("Default"));

            Rigidbody rb = itemToDrop.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                // rb.AddForce(Camera.main.transform.forward * 5f, ForceMode.Impulse);
                rb.AddForce(holdPoint.forward * 5f, ForceMode.Impulse);
            }

            // LOGIC MỚI: Sắp xếp lại tay cầm sau khi vứt
            if (inventory.Count == 0) 
            {
                // Túi trống trơn
                selectedIndex = -1; 
            }
            else 
            {
                // Còn đồ trong túi, tự động lùi về món trước đó
                selectedIndex--;
                if (selectedIndex < 0) selectedIndex = inventory.Count - 1;
                EquipCurrentItem();
            }
        }
    }
}