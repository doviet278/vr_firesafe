using System;
using UnityEngine;

public class ItemNoSelect : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject losePopup;
    public string GetInteractPrompt()
    {
        return "Cannot interact with this item.";
    }

    public void OnInteract()
    {
        Time.timeScale = 0f; // Tạm dừng game
        losePopup.SetActive(true);
    }
}
