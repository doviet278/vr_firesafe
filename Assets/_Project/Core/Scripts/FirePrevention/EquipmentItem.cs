using UnityEngine;
using UnityEngine.UI;

public class EquipmentItem : MonoBehaviour
{
    public bool isFound = false;

    [Header("UI Tick")]
    public Image tickIcon; 

    private SceneFirePrevention gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<SceneFirePrevention>();
        if (tickIcon != null)
            tickIcon.gameObject.SetActive(false); 
    }
    public void Interact()
    {
        gameManager.OnEquipmentFound(this);
    }

    public void MarkFound()
    {
        isFound = true;

        if (tickIcon != null)
            tickIcon.gameObject.SetActive(true);
    }
}
