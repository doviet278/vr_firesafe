using UnityEngine;

public class Scene3BManager : MonoBehaviour
{
    public static Scene3BManager Instance;

    private bool usedPhone = false;
    private bool usedCloth = false;
    private bool usedAlarm = false;
    public bool isSceneCompleted = false;
    void Awake()
    {
        Instance = this;
    }

    public void MarkItemUsed(ItemType type)
    {
        switch (type)
        {
            case ItemType.Phone:
                usedPhone = true;
                break;

            case ItemType.Use: // khăn
                usedCloth = true;
                break;
        }

        CheckComplete();
    }

    void CheckComplete()
    {
        if (usedCloth && usedAlarm)
        {
            OnSceneCompleted();
        }
    }

    void OnSceneCompleted()
    {
        isSceneCompleted = true;
    }

    public void SetUsedAlarm()
    {
        usedAlarm = true;
        CheckComplete();
    }
}