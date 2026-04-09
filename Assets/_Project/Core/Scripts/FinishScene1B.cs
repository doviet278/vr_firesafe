using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class FinishScene1B : MonoBehaviour
{
    public static FinishScene1B Instance;
    public int totalFireItems = 27;
    [SerializeField] private GameObject completedPopup;

    private int extinguishedCount = 0;

    public bool isCompleted = false;

    private void Awake()
    {
        Instance = this;
    }
    // Gọi hàm này mỗi khi dập 1 item
    public void ReportFireExtinguished()
    {
        if (isCompleted) return;

        extinguishedCount++;
        Debug.LogError(extinguishedCount);

        if (extinguishedCount >= totalFireItems)
        {
            CompleteRoom();
        }
    }

    void CompleteRoom()
    {
        isCompleted = true;
        Debug.LogError($"HOAN THANH!");
        completedPopup.SetActive(true);
    }
}
