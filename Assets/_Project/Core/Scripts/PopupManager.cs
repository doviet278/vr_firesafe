using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private Button btnStart;
    [SerializeField] private Transform newPositionPopup;

    private void Start()
    {
        btnStart.onClick.AddListener(ShowPopup);
    }

    private void ShowPopup()
    {
        btnStart.gameObject.SetActive(false);
        transform.position = newPositionPopup.position;
        Debug.LogError("clickPopup");
        item.SetActive(true);
    }

    private void OnDestroy()
    {
        btnStart.onClick.RemoveListener(ShowPopup);
    }

}
