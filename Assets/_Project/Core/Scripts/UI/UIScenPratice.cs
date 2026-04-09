using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScenPratice : MonoBehaviour
{
    public static UIScenPratice Instance { get; private set; }

    [SerializeField] private GameObject winPopup;
    [SerializeField] private GameObject losePopup;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        winPopup.SetActive(false);
        losePopup.SetActive(false);
    }

    public void ShowWinPopup()
    {
        winPopup.SetActive(true);
    }

    public void HideWinPopup()
    {
        winPopup.SetActive(false);
    }

    public void ShowLosePopup()
    {
        losePopup.SetActive(true);
    }

    public void HideLosePopup()
    {
        losePopup.SetActive(false);
    }

    public void ChangeSceneMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Replay(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
