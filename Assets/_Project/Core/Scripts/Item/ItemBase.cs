using UnityEngine;

public abstract class ItemBase : MonoBehaviour, IInteractable
{
    [Header("Item Info")]
    public string itemName = "Vat pham co ban";
    public float itemWeight = 1.5f;

    protected bool isEquipped = false;

    public string GetInteractPrompt()
    {
        return $"Nhan F de nhat {itemName} ({itemWeight}kg)";
    }

    public void OnInteract()
    {
        InventoryManager.Instance.PickupItem(this);
    }

    public virtual void OnEquip() 
    {
        isEquipped = true;
        gameObject.SetActive(true); 
        // Thay đổi Layer cho toàn bộ vật thể cha và con
        ChangeLayerRecursively(gameObject, LayerMask.NameToLayer("Ignore Raycast")); 
    }

    public virtual void OnUnequip() 
    {
        isEquipped = false;
        gameObject.SetActive(false); 
    }

    public virtual void UseItem() 
    {
        Debug.Log($"Dang su dung: {itemName}");
    }

    // Hàm tiện ích: Đổi Layer đệ quy
    public void ChangeLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            ChangeLayerRecursively(child.gameObject, newLayer);
        }
    }
}