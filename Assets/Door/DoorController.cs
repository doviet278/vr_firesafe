using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public Animator animator;
    public string openTriggerName = "Open";

    private bool isOpen = false;
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public string GetInteractPrompt()
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        if (isOpen) return;

        if (animator != null)
        {
            animator.SetTrigger(openTriggerName);
            isOpen = true;
            boxCollider.enabled = false;
        }
        else
        {
            Debug.LogWarning("Animator chưa được gán cho Door!");
        }
    }
}