using UnityEngine;

public class UISceneTrain : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }
}
