using UnityEngine;

public class ItemSelectScene2A : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject popup;
    public string GetInteractPrompt()
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        popup.SetActive(true);
    }
}
