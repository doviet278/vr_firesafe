using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float interactRange = 3f;

    [Header("UI Crosshair")]
    public Image crosshairImage;
    public Sprite defaultCrosshair;
    public Sprite interactCrosshair;

    private IInteractable currentInteractable;

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;
        HandleRaycast();
        HandleInput();
    }

    private void HandleRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                crosshairImage.sprite = interactCrosshair;
                return;
            }
        }
        currentInteractable = null;
        crosshairImage.sprite = defaultCrosshair;
    }

    private void HandleInput()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame && currentInteractable != null)
        {
            currentInteractable.OnInteract();
        }
    }
}