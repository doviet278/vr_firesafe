using UnityEngine;

public class SceneTrainExtinguisherManager : MonoBehaviour
{
    public static SceneTrainExtinguisherManager Instance;
    public int totalFireItems = 12; 
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

    }
}
