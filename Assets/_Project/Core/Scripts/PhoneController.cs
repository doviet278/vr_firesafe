using UnityEngine;
using TMPro;
using System.Collections; 

public class PhoneController : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    public string targetNumber = "114";
    public int maxFails = 3;

    [Header("UI UI")]
    public GameObject phoneUIPanel;
    public TMP_Text screenText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip successSound;
    public AudioClip wrongNumberVoice;

    private string currentInput = "";
    private int failCount = 0;
    private bool isCalling = false; 

    void Start()
    {
        if (phoneUIPanel != null) phoneUIPanel.SetActive(false);
    }

    public string GetInteractPrompt() => "Nhấn F để sử dụng điện thoại";

    public void OnInteract()
    {
        isCalling = false;
        currentInput = "";
        if (screenText != null) screenText.text = ""; 
        
        phoneUIPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TypeNumber(string num)
    {
        if (isCalling) return; 

        if (screenText.text == "Gọi sai số" || screenText.text == "Vui lòng gọi số cứu hỏa là 114")
        {
            currentInput = "";
        }

        if (currentInput.Length < 11)
        {
            currentInput += num;
            screenText.text = currentInput;
        }
    }

    public void ClearNumber()
    {
        if (isCalling) return;
        currentInput = "";
        screenText.text = currentInput;
    }

    public void PressCall()
    {
        if (isCalling || currentInput == "") return; 

        if (currentInput == targetNumber)
        {
            Debug.Log("Đã gọi đúng 114!");
            isCalling = true; // Khóa bàn phím
            
            if (successSound) audioSource.PlayOneShot(successSound);
            
            screenText.text = "Đã gọi đúng";
            GameTaskManager.Instance.MarkTaskCompleted("2");
            StartCoroutine(ClosePhoneAfterDelay(2f));
        }
        else
        {
            failCount++;
            if (failCount >= maxFails)
            {
                screenText.text = "Vui lòng gọi số cứu hỏa là 114";
                currentInput = ""; 
                failCount = 0; 
            }
            else
            {
                if (wrongNumberVoice) audioSource.PlayOneShot(wrongNumberVoice);
                screenText.text = "Gọi sai số";
                currentInput = ""; 
            }
        }
    }

    private IEnumerator ClosePhoneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        ClosePhone();
    }

    public void ClosePhone()
    {
        isCalling = false;
        phoneUIPanel.SetActive(false);
        
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}