using UnityEngine;

public class DoorGoOut : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject winPopup;
    [SerializeField] private GameObject losePopup;

    public string GetInteractPrompt()
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        if(Scene3BManager.Instance.isSceneCompleted)
        {
            winPopup.SetActive(true);
        }
        else
        {
            losePopup.SetActive(true);
        }

        // Dừng toàn bộ thời gian vật lý và chuyển động trong game
        Time.timeScale = 0f;

        // Hiển thị lại con trỏ chuột để người chơi có thể bấm nút
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
