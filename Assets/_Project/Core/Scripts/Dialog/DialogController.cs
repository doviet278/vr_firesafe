using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public DialogLine[] lines;
    public float typingSpeed = 0.03f;
    [SerializeField] private PlayerMovement pcController;
    [SerializeField] private GameObject video;

    private int index = 0;
    private bool isTyping = false;
    private bool textFullyShown = false;

    private Coroutine typingCoroutine;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (pcController != null)
        {
            pcController.allowLook = false;
        }
        nextButton.onClick.AddListener(clickNextBtn);
    }
    void Start()
    {
        ShowLine(index);
    }

    private void clickNextBtn()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = lines[index].text;
            isTyping = false;
            textFullyShown = true;
        }
        else
        {
            NextLine();
        }
    }

    void ShowLine(int i)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(lines[i].text));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        textFullyShown = false;

        dialogueText.text = "";

        foreach (char c in sentence)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        textFullyShown = true;
    }

    void NextLine()
    {
        if (!textFullyShown) return;

        index++;

        if (index < lines.Length)
        {
            ShowLine(index);
        }
        else
        {
            gameObject.SetActive(false);
            if(video != null) video.SetActive(true);
            if (GameManager.Instance != null) GameManager.Instance.isShowDialog = false;
        }
    }

    private void OnDisable()
    {
        nextButton.onClick.RemoveListener(clickNextBtn);

        if (pcController != null)
        {
            pcController.allowLook = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
}