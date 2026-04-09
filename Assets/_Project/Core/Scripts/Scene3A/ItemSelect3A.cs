using UnityEngine;

public class ItemSelect3A : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject nextArrow;

    public string GetInteractPrompt()
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        popup.SetActive(true);
        if (nextArrow != null)
        {
            nextArrow.SetActive(true);
        }
    }

    
}
