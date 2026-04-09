using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float interactRange = 3f;

    [Header("UI Crosshair")]
    public Image crosshairImage;
    public Sprite defaultCrosshair;
    public Sprite interactCrosshair;
    private UITutorial uiTutorial;
    private IInteractable currentInteractable;

    private void Awake()
    {
        GameObject obj = GameObject.Find("UITutorial");
        if (obj != null)
        {
            uiTutorial = obj.GetComponent<UITutorial>();
        }
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;
        HandleRaycast();
        HandleInput();
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleUIClick();
            HandleSelect();
        }
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
                uiTutorial?.ShowTutorialF();
                return;
            }
         
        }
        uiTutorial?.HideTutorialF();
        currentInteractable = null;
        crosshairImage.sprite = defaultCrosshair;
    }

    private void HandleSelect()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            EquipmentItem item = hit.collider.GetComponent<EquipmentItem>();
            if (item != null)
            {
                item.Interact();
            }
        }
    }

    private void HandleUIClick()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = new Vector2(Screen.width / 2, Screen.height / 2);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            Button btn = result.gameObject.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.Invoke();
                return;
            }
        }
    }

    private void HandleInput()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame && currentInteractable != null)
        {
            currentInteractable.OnInteract();
        }
    }
}