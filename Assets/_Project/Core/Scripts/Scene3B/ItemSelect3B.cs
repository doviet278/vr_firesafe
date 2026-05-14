using UnityEngine;

public class ItemSelect3B : ItemBase
{
    [Header("Item Type")]
    public ItemType itemType;

    [Header("Hold Position")]
    public Transform holdPoint; // vị trí cầm (gắn vào camera)

    public override void OnEquip()
    {
        base.OnEquip();

        Transform playerCam = Camera.main.transform;
        transform.SetParent(playerCam);

        // Đặt vị trí trước mặt
        transform.localPosition = new Vector3(0.3f, -0.3f, 0.5f);
        transform.localRotation = Quaternion.identity;
    }

    public override void UseItem()
    {
        base.UseItem();

        Debug.Log("Dang su dung item: " + itemType);

        switch (itemType)
        {
            case ItemType.Phone:
                UsePhone();
                break;

            case ItemType.Use:
                UseCloth();
                break;
        }

        Scene3BManager.Instance.MarkItemUsed(itemType);
        InventoryManager.Instance.RemoveCurrentItem();
    }

    void UsePhone()
    {
        Debug.Log("Dang dung dien thoai ");
    }

    void UseCloth()
    {
        Debug.Log("Dang che mat bang khan ");
        TowelUIManager.Instance.ShowClothOverlay(true);
    }

}

public enum ItemType
{
    Use,
    Phone
}