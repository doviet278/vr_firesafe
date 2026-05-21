using UnityEngine;

public class EndGame : MonoBehaviour
{
    private bool isEndGameTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isEndGameTriggered) return;

        if (other.CompareTag("EndGameTrigger"))
        {
            isEndGameTriggered = true;
            TriggerEndGame();
        }
    }

    private void TriggerEndGame()
    {
        Time.timeScale = 0f; 

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameTaskManager.Instance.MarkTaskCompleted("5");
        if (UIScenPratice.Instance != null)
        {
            UIScenPratice.Instance.ShowWinPopup();
        }
    }
}
