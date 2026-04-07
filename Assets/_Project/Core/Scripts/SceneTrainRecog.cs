using UnityEngine;

public class SceneTrainRecog : MonoBehaviour
{
    private int itemCount = 3;
    private int item;
    private bool completed;
    private void Awake()
    {
        item = 0;
    }
    public void CheckCompleted()
    {
        if(completed) return;
        item++;
        if(item >= itemCount)
        {
            completed = true;
            ShowCompletedUI();
        }
    }

    public void ShowCompletedUI()
    {
        Debug.LogError("SceneTrainRecog Completed!");
    }
}
