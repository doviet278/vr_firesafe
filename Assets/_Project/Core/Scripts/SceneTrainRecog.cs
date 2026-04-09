using UnityEngine;
using UnityEngine.InputSystem;

public class SceneTrainRecog : MonoBehaviour
{
    [SerializeField] private GameObject popupCompleted;
    [SerializeField] private GameObject txtNoti;
    private int itemCount = 3;
    private int item;
    private bool completed;
    private void Awake()
    {
        item = 0;
    }

    private void Update()
    {
        if (completed)
        {
            if(Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                ShowCompletedUI();
            }
        }
    }
    public void CheckCompleted()
    {
        if(completed) return;
        item++;
        if(item >= itemCount)
        {
            completed = true;
            txtNoti.SetActive(true);
        }
    }

    public void ShowCompletedUI()
    {
        popupCompleted.SetActive(true);
        Time.timeScale = 0f;

        // Hiển thị lại con trỏ chuột để người chơi có thể bấm nút
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
